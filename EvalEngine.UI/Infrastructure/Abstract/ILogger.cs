// -----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="MPR INC">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Infrastructure.Abstract
{
    /// <summary>
    /// An interface for the logger that is to be used site wide.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Stores a message in the log file.
        /// </summary>
        /// <param name="message">The message to be stored in the log file.</param>
        void Info(string message);

        /// <summary>
        /// Adds an Error message to the log file.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);
    }
}
