using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Watchdog.Engine
{
    public class ErrorHandlingEngine : HeimdallEngine
    { 
        
        protected override void OnProcess(ApplicationLogEntry entry)
        {

        }
        public ErrorHandlingEngine(IObservable<ApplicationLogEntry> observable)
            : base(observable)
        {

        }

        protected override Func<ApplicationLogEntry, bool> SubscriptionPredicate
        {
            get { return e=>e is ApplicationErrorLogEntry; }
        }
    }
}