// -----------------------------------------------------------------------
// <copyright file="NLogLogger.cs" company="MPR INC">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Infrastructure.Concrete
{
    using EvalEngine.Infrastructure.Abstract;
    using NLog;

    /// <summary>
    /// A logger implementing the ILogger interface and
    /// using the NLog library.
    /// </summary>
    public class NLogLogger : ILogger
    {
        /// <summary>
        /// The object managing how to log site activity.
        /// </summary>
        private Logger logger;

        /// <summary>
        /// Initializes a new instance of the NLogLogger class.
        /// </summary>
        public NLogLogger(string currentClassName)
        {
           this.logger = LogManager.GetLogger(currentClassName);
        }

        /// <summary>
        /// Adds a message to the log file.
        /// </summary>
        /// <param name="message">The message to be put in the log file.</param>
        public void Info(string message)
        {
            this.logger.Info(message);
        }

        /// <summary>
        /// Adds an Error message to the log file.
        /// </summary>
        /// <param name="message">The message to be put in the log file.</param>
        public void Error(string message)
        {
            this.logger.Error(message);
        }
    }
}