using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Watchdog.Engine
{
    public class ApplicationLogEntry
    {
        public string ApplicationId { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime OccurredOn { get; set; }
    }
    public class ApplicationErrorLogEntry : ApplicationLogEntry
    {
        public string Error { get; set; }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
}