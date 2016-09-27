// -----------------------------------------------------------------------
// <copyright file="PasswordHistoryTest.cs" company="RTI, Inc.">
// Password history tests
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Test
{
    using EvalEngine.Domain.Abstract;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Password history test class
    /// </summary>
    [TestFixture]
    public class PasswordHistoryTest
    {
        /// <summary>
        /// mock password history repository
        /// </summary>
        public Mock<IPasswordHistoryRepository> mockPasswordHistory = new Mock<IPasswordHistoryRepository>();

        /// <summary>
        /// SetUp method
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }

        /// <summary>
        /// Cannot use password in history test
        /// </summary>
        [Test]
        public void Can_not_use_password_in_history()
        {  
        }
    }
}
