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

namespace Watchdog.Engine
{
    public class DefaultEntryHandler : HeimdallEngine
    {
        private bool disposed = false;
        private Task loggingTask = null;
        private static BlockingCollection<ApplicationLogEntry> logEntryQueue = new BlockingCollection<ApplicationLogEntry>(new ConcurrentQueue<ApplicationLogEntry>());
        private void LogMessages()
        {
            string loggingDirectory = WebConfigurationManager.AppSettings["LoggingDirectory"];
            if(!Directory.Exists(loggingDirectory))
            {
                Directory.CreateDirectory(loggingDirectory);
            }
            while(!logEntryQueue.IsCompleted)
            {
                try
                {
                    ApplicationLogEntry entry = logEntryQueue.Take();
                    string path = Path.Combine(loggingDirectory, string.Format("{0}.xml", entry.ApplicationId));

                    if (!File.Exists(path))
                    {
                        File.WriteAllText(path, "<logEntries></logEntries>");
                    }

                    //using(Stream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    //{
                        XDocument doc = XDocument.Load(path);
                        doc.Root.Add(entry.ToXElement());
                        doc.Save(path);
                    //}
                }
                catch(Exception e)
                {
                    //TODO: Handle this
                }
            }
        }
        protected override void OnProcess(ApplicationLogEntry entry)
        {
            logEntryQueue.Add(entry);
        }
        private IDisposable BiFrost { get; set; }

        protected new void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if(!disposed)
            {
                if(disposing)
                {

                    logEntryQueue.CompleteAdding();
                    loggingTask.Wait();

                    loggingTask.Dispose();
                    logEntryQueue.Dispose();
                    
                }
                disposed = true;
            }
        }
        protected override Func<ApplicationLogEntry, bool> SubscriptionPredicate
        {
            get { return e => true; }
        }
        public DefaultEntryHandler(IObservable<ApplicationLogEntry> observable)
            : base(observable)
        {

            loggingTask = Task.Factory.StartNew(LogMessages);
        }
        ~DefaultEntryHandler()
        {
            OnDispose(false);
        }
        public new void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }
    }
}