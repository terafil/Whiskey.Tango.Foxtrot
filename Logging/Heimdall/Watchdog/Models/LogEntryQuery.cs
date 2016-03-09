using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Watchdog.Models
{
    public class LogEntryQuery
    {
        DateTime? occurredAfter { get; set; }
        DateTime? occurredBefore { get; set; }
    }
}