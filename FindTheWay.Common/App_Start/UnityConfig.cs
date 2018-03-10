using System.Web.Mvc;
using FindTheWay.Common.Services;
using FindTheWay.Common.Services.Impl;
using FindTheWay.Common.Unity;
using FindTheWay.Data;
using Microsoft.Practices.Unity;

namespace FindTheWay.Common
{
	public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<FindTheWayDbContext>(new PerRequestLifetimeManager(), new InjectionConstructor());
            container.RegisterType<IMapService, MapService>();

            DependencyResolver.SetResolver(new global::Unity.Mvc5.UnityDependencyResolver(container));
        }
    }
}