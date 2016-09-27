// -----------------------------------------------------------------------
// <copyright file="SqlPasswordHistoryRepository.cs" company="RTI, Inc.">
// Job Message repository
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Concrete
{
    using System;
    using System.Configuration;
    using System.Data.Linq;
    using System.Linq;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Entities;

    /// <summary>
    /// Password history repository
    /// </summary>
    public class SqlPasswordHistoryRepository : Repository<PasswordHistory>, IPasswordHistoryRepository
    {
        /// <summary>
        /// The password history repository.
        /// </summary>
        private readonly Table<PasswordHistory> passwordHistoryTable;

        /// <summary>
        /// The user account info repository.
        /// </summary>
        private readonly Table<UserAccountInfo> userAccountInfoTable;

        /// <summary>
        /// number of generations of passwords to keep.
        /// </summary>
        private readonly int numberOfGenerations;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlPasswordHistoryRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public SqlPasswordHistoryRepository(string connectionString)
            : this(connectionString, 24)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlPasswordHistoryRepository" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="numberOfGenerations">number of generations of passwords to save</param>
        public SqlPasswordHistoryRepository(string connectionString, int numberOfGenerations)
            : base(connectionString)
        {
            // Ensure number of generations is at least 1.
            this.numberOfGenerations = numberOfGenerations >= 1 ? numberOfGenerations : 1;
            DataContext dc = new DataContext(connectionString);
            this.passwordHistoryTable = dc.GetTable<PasswordHistory>();
            this.userAccountInfoTable = dc.GetTable<UserAccountInfo>();
        }

        /// <summary>
        /// The check password history
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">password to test</param>
        /// <param name="salt">salt from user</param>
        /// <returns>true if user can use password, false otherwise</returns>
        public bool CanUsePassword(Guid userId, string password, string salt)
        {
            var passwords = this.passwordHistoryTable.Where(x => x.UserId.Equals(userId));

            return !passwords.Select(x => x.Password).Contains(HashPassword(password, salt));
        }

        /// <summary>
        /// The update password history.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">password to add to history</param>
        /// <param name="salt">salt for password</param>
        public void UpdatePasswordHistory(Guid userId, string password, string salt)
        {
            if (this.CanUsePassword(userId, password, salt))
            {
                var passwords = this.passwordHistoryTable.Where(x => x.UserId.Equals(userId));
                if (passwords.Count() >= this.numberOfGenerations)
                {
                    var passwordsToDelete = passwords.OrderByDescending(x => x.LastUsed).Skip(this.numberOfGenerations - 1);
                    this.passwordHistoryTable.DeleteAllOnSubmit(passwordsToDelete);
                }

                this.passwordHistoryTable.InsertOnSubmit(new PasswordHistory { LastUsed = DateTime.Now, Password = HashPassword(password, salt), UserId = userId });
                this.passwordHistoryTable.Context.SubmitChanges();
            }
        }

        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="unencryptedPassword">The unencrypted password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        public string HashPassword(string unencryptedPassword, string salt)
        {
            System.Security.Cryptography.SHA512CryptoServiceProvider x = new System.Security.Cryptography.SHA512CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(unencryptedPassword + salt);
            data = x.ComputeHash(data);
            return System.Convert.ToBase64String(data);
        }


        /// <summary>
        /// Delete a user's password history.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void DeleteByUserId(Guid userId)
        {
            var histories = this.passwordHistoryTable.Where(x => x.UserId.Equals(userId));
            this.passwordHistoryTable.DeleteAllOnSubmit(histories);
            this.passwordHistoryTable.Context.SubmitChanges();
        }
    }
}