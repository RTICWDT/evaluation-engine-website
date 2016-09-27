// -----------------------------------------------------------------------
// <copyright file="SqlUserAccountInfoRepository.cs" company="MPR INC">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Linq;
    using EvalEngine.Domain.Entities;
    using System.Security.Cryptography;
    using EvalEngine.Domain.Abstract;
    using System.Configuration;


    /// <summary>
    /// Class SqlUserAccountInfoRepository.
    /// </summary>
    public class SqlUserAccountInfoRepository : Repository<UserAccountInfo>, IUserAccountInfoRepository
    {
        /// <summary>
        /// The user account ifno table.
        /// </summary>
        private readonly Table<UserAccountInfo> userAccountInfoTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStateAssignmentRepository" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlUserAccountInfoRepository(string connectionString)
            : base(connectionString)
        {
            DataContext dc = new DataContext(connectionString);
            this.userAccountInfoTable = dc.GetTable<UserAccountInfo>();
        }

        /// <summary>
        /// Delete a user account
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        public void DeleteUserAccountInfoByUserId(Guid userId)
        {
            var account = base.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
            if (account != null)
            {
                base.Delete(account);
            }
        }

        /// <summary>
        /// The create user account info
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void CreateUserAccountInfo(Guid userId)
        {
            UserAccountInfo account = new UserAccountInfo
            {
                ResetToken = UserAccountInfo.GenerateToken(),
                ResetTime = DateTime.Now,
                ResetFlag = false,
                VerifyToken = UserAccountInfo.GenerateToken(),
                VerifyTime = DateTime.Now,
                VerifyFlag = true,
                UserId = userId,
                Salt = GetRandomBase64String(10)
            };

            base.Update(account);
        }

        /// <summary>
        /// The generate random base 64 string
        /// </summary>
        /// <param name="length">length of the string to generate</param>
        /// <returns>A random string</returns>
        public string GetRandomBase64String(int length)
        {
            Byte[] randomBytes = new Byte[length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            return System.Convert.ToBase64String(randomBytes);
        }
    }
}
