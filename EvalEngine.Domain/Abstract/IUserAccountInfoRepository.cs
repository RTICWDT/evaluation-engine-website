// -----------------------------------------------------------------------
// <copyright file="IUserAccountInfoRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Abstract
{
    using EvalEngine.Domain.Entities;
    using System;

    /// <summary>
    /// The user account info repository interface
    /// </summary>
    public interface IUserAccountInfoRepository : IRepository<UserAccountInfo>
    {
        /// <summary>
        /// The delete account info for user by user ID
        /// </summary>
        /// <param name="userName">
        /// The userName to delete.
        /// </param>
        void DeleteUserAccountInfoByUserId(Guid userId);

        /// <summary>
        /// The create user account info
        /// </summary>
        /// <param name="userName">
        /// The user account info record to create
        /// </param>
        void CreateUserAccountInfo(Guid userId);

        /// <summary>
        /// The generate random base 64 string
        /// </summary>
        /// <param name="length">
        /// length of the string to generate
        /// </param>
        /// <returns>A random string</returns>
        string GetRandomBase64String(int length); 
    }
}