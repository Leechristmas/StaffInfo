using System;
using NLog;

namespace Staffinfo.API.Infrastructure
{
    public static class LogHelper
    {
        /// <summary>
        /// Writes a log-message
        /// </summary>
        /// <param name="logger">a logger</param>
        /// <param name="level">logging level</param>
        /// <param name="message">log-message</param>
        /// <param name="loggerName">a logger name</param>
        /// <param name="userId">an id of the current user</param>
        /// <param name="issue">additional info</param>
        public static void Log(this ILogger logger, Guid userId, LogLevel level, string message, string loggerName, string issue)
        {
            LogEventInfo logEvent = new LogEventInfo(level, loggerName, message);
            logEvent.Properties["UserId"] = userId;
            logEvent.Properties["Issue"] = issue;

            logger.Log(logEvent);
        }
    }
}