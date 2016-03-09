using Microsoft.Practices.Unity;
using System;
using System.Reactive.Subjects;
using System.Web.Http;
using Unity.WebApi;
using Watchdog.Engine;

namespace Watchdog
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IObservable<ApplicationLogEntry>, Subject<ApplicationLogEntry>>(new ContainerControlledLifetimeManager());
            container.RegisterType<IObserver<ApplicationLogEntry>, Subject<ApplicationLogEntry>>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHeimdallEngine, DefaultEntryHandler>("default", new ContainerControlledLifetimeManager());
            container.RegisterType<IHeimdallEngine, ErrorHandlingEngine>("errorHandler", new ContainerControlledLifetimeManager());
            
            config.DependencyResolver = new UnityDependencyResolver(container);

            //Start a new heimdall engine.
            container.ResolveAll<IHeimdallEngine>();
        }
    }
}