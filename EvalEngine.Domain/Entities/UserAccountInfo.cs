// -----------------------------------------------------------------------
// <copyright file="UserAccountInfo.cs" company="MPR INC">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    using System;
    using System.Data.Linq.Mapping;
    using System.Text;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Concrete;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Table(Name = "UserAccountInfo")]
    public class UserAccountInfo : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the verify flag
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool VerifyFlag { get; set; }

        /// <summary>
        /// Gets or sets the verify GUID
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string VerifyToken { get; set; }

        /// <summary>
        /// Gets or sets the latest verify time.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime VerifyTime { get; set; }

        /// <summary>
        /// Gets or sets the reset flag
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool ResetFlag { get; set; }

        /// <summary>
        /// Gets or sets the reset GUID.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ResetToken { get; set; }

        /// <summary>
        /// Gets or sets the last reset time.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime ResetTime { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Salt { get; set; }

        /// <summary>
        /// Generates both reset and verify tokens
        /// </summary>
        public void GenerateTokens()
        {
            this.GenerateResetToken();
            this.GenerateVerifyToken();
        }

        /// <summary>
        /// Generates a token to be sent to the users when reseting their password.
        /// </summary>
        public void GenerateResetToken()
        {
            this.ResetToken = GenerateToken();
            this.ResetTime = DateTime.Now;
            this.ResetFlag = true;
        }

        public void GenerateVerifyToken()
        {
            this.VerifyToken = GenerateToken();
            this.VerifyTime = DateTime.Now;
            this.VerifyFlag = true;
        }

        /// <summary>
        /// Generates a token using a HMACSHA512 algorithm.
        /// </summary>
        /// <returns>A token to be used in a URL.</returns>
        public static string GenerateToken()
        {
            var hashAlg = new MPRHMACSHA512(Convert.FromBase64String(ConfigurationManager.AppSettings["Key64"]));

            byte[] text = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() + "^p6t0t0s%N*s@___");
            byte[] hash = hashAlg.ComputeHash(text);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Was the token generated more than one hour ago?
        /// </summary>
        /// <returns>True if the token was generated more than an hour ago. False otherwise.</returns>
        public bool HasResetTokenExpired()
        {
            return this.HasResetTokenExpired(60);
        }

        /// <summary>
        /// Was the token generated more than a certain number of minutes?
        /// </summary>
        /// <param name="minutesBeforeExpiration">Number of minutes before the token expires.</param>
        /// <returns>True if the token was generated more than an minutesBeforeExpiration. False otherwise.</returns>
        public bool HasResetTokenExpired(double minutesBeforeExpiration)
        {
            var timeSinceIssued = DateTime.Now.Subtract(this.ResetTime).TotalMinutes;
            return timeSinceIssued > minutesBeforeExpiration;
        }
    }
}
