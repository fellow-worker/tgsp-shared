using System;
using Microsoft.Extensions.Logging;

namespace TGSP.Shared.Testing
{
    /// <summary>
    /// This class helps in testing when a ILogger is needed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogEntry
    {
        /// <summary>
        /// The log level that was used
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Identifies the logging event.
        /// </summary>
        public EventId EventId { get; set; }

        /// <summary>
        /// The logged state
        /// </summary>
        public object State { get; set; }

        /// <summary>
        /// A logged exception
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// The message that was provider (when the formatter could be used)
        /// </summary>
        public string Message { get; set; }
    }
}