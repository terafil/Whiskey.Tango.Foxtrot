using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Watchdog.Models
{
    public class LogEntryModel
    {
        public string message { get; set; }
        public DateTime occurredOn { get; set; }
    }
    
}