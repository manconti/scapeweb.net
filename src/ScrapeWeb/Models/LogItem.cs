using System;
namespace ScrapeWeb.Models
{
    public enum LogType
    {
        Info = 0,
        Message = 1,
        Error = 2,
        Warning = 3,
        Success = 4
    }

    public sealed class LogItem
    {
        /// <summary>
        /// Get the #<see cref="LogType"/> of the log item.
        /// </summary>
        public LogType Type { get; internal set; }
        /// <summary>
        /// Get the Message of the Log Item
        /// </summary>
        public string Message { get; internal set; }
        /// <summary>
        /// Get the UTC Date and Time of when Log Item has been created;
        /// </summary>
        public DateTime DateTime { get; internal set; }

        public LogItem()
        {
        }
    }
}
