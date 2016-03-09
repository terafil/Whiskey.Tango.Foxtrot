using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Web.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace Watchdog.Engine
{
    public abstract class HeimdallEngine : IHeimdallEngine
    {
        private bool disposed = false;
       
        private void Process(ApplicationLogEntry entry)
        {
            OnProcess(entry);
        }
        private IDisposable BiFrost { get; set; }

        protected abstract void OnProcess(ApplicationLogEntry entry);
        protected abstract Func<ApplicationLogEntry, bool> SubscriptionPredicate { get; }

        protected void OnDispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    BiFrost.Dispose();
                }
                disposed = true;
            }
        }
        protected HeimdallEngine(IObservable<ApplicationLogEntry> observable)
        {
            if (observable == null)
            {
                throw new ArgumentNullException("observable");
            }
            BiFrost = observable.SubscribeOn(TaskPoolScheduler.Default).Where(SubscriptionPredicate).Subscribe(e => Process(e));
        }
        ~HeimdallEngine()
        {
            OnDispose(false);
        }
        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }
    }
}