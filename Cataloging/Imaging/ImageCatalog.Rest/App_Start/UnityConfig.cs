using ImageCatalog.Rest.Services;
using Microsoft.Practices.Unity;
using System;
using System.Reactive.Subjects;
using System.Web.Http;
using Unity.WebApi;

namespace ImageCatalog.Rest
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IObservable<PendingImage>, Subject<PendingImage>>(new ContainerControlledLifetimeManager())
                .RegisterType<IObserver<PendingImage>, Subject<PendingImage>>(new ContainerControlledLifetimeManager())
                .RegisterType<IImageProcessor, ImageProcessor>(new ContainerControlledLifetimeManager());


            config.DependencyResolver = new UnityDependencyResolver(container);

            //Startup image processor
            container.Resolve<IImageProcessor>().Start();
        }
    }
}