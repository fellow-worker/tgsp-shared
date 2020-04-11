using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace TGSP.Shared.Testing
{
    /// <summary>
    /// This class helps in testing when a ILogger is needed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Logger<T> : ILogger<T>, IDisposable
    {
        /// <summary>
        /// A list of all the entries logged by this logger
        /// </summary>
        public List<LogEntry> LogEntires { get; set; } = new List<LogEntry>();

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">unction to create a System.String message of the state and exception.</param>
        /// <typeparam name="TState">The type of the object to be written.</typeparam>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = string.Empty;
            if(formatter != null) message = formatter(state, exception);
            var entry = new LogEntry { LogLevel = logLevel, EventId = eventId, State = state , Exception = exception , Message = message };
            LogEntires.Add(entry);
        }

        /// <summary>
        /// Checks if the given logLevel is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns>true if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
        /// <returns>An System.IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state) => this;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Clear all the log entires logged until this point
        /// </summary>
        public void Clear() => LogEntires.Clear();

    }
}
