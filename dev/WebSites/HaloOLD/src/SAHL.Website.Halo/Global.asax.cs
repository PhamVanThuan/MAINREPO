using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SAHL.Core.Configuration;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.IoC;
using StructureMap;

namespace SAHL.Website.Halo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ViewEngineConfig.RegisterViewEngine(ViewEngines.Engines);
			ModelBinderConfig.Configure(System.Web.Mvc.ModelBinders.Binders);

            string whatisit = ObjectFactory.WhatDoIHave();
            var startables = ObjectFactory.GetAllInstances<IStartable>();
            foreach (var startable in startables)
            {
                startable.Start();
            }

            DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
        }
    }
}