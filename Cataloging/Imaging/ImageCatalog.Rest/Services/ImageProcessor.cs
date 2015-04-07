using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.IO;

namespace ImageCatalog.Rest.Services
{
    public class ImageProcessor : IImageProcessor
    {
        private void Process(PendingImage pending)
        {
            string fileName = pending.FileName;
            foreach(char invalid in Path.GetInvalidFileNameChars())
            {
                if(fileName.Contains(invalid))
                {
                    // doing tostring because you can replace with an empty character literal.
                    fileName = fileName.Replace(invalid.ToString(), "");
                }
            }
            string newFilePath = Path.Combine(Path.GetDirectoryName(pending.SavedFile), fileName);
            File.Move(pending.SavedFile, newFilePath);
        }
        private IObservable<PendingImage> Observable { get; set; }
        private IDisposable Subscription { get; set; }
        private bool Disposed { get; set; }

        protected virtual void OnDispose(bool disposing)
        {
            if(!Disposed)
            {
                if(disposing)
                {
                    Subscription.Dispose();
                }
                Disposed = true;
            }
        }

        public ImageProcessor(IObservable<PendingImage> observable)
        {
            Observable = observable;
        }
        ~ImageProcessor()
        {
            OnDispose(false);
        }
        public void Start()
        {
            Subscription = Observable.Subscribe(Process);
        }

        public void Dispose()
        {
            OnDispose(true);
        }
    }

    public class PendingImage
    {
        public string SavedFile {get; set;}
        public string FileName {get; set; }
    }
}