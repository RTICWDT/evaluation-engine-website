// -----------------------------------------------------------------------
// <copyright file="IPasswordHistoryRepository.cs" company="RTI, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Domain.Abstract
{
    using EvalEngine.Domain.Entities;
    using System;

    /// <summary>
    /// The password history repository interface.
    /// </summary>
    public interface IPasswordHistoryRepository : IRepository<PasswordHistory>
    {
        /// <summary>
        /// Checks if user can use specific password
        /// </summary>
        /// <param name="username">username to test against</param>
        /// <param name="password">password to test</param>
        /// <param name="salt">salt from user</param>
        /// <returns>true if user can use password, false otherwise</returns>
        bool CanUsePassword(Guid userId, string password, string salt);

        /// <summary>
        /// Updates password history table, deletes a record and inserts if at max password limit, otherwise just inserts password.
        /// </summary>
        /// <param name="username">username to update passwords for</param>
        /// <param name="password">password to add to history</param>
        /// <param name="salt">salt for password</param>
        void UpdatePasswordHistory(Guid userId, string password, string salt);

        /// <summary>
        /// Delete a user's password history.
        /// </summary>
        /// <param name="userId"></param>
        void DeleteByUserId(Guid userId);
    }
}
