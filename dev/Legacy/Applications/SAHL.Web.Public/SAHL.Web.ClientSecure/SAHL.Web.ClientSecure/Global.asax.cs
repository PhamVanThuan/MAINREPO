using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SAHL.Web.ClientSecure.Helpers;
using Ninject;
using System.Reflection;
using Ninject.Web.Mvc;
using SAHL.Web.ClientSecure.Models;

namespace SAHL.Web.ClientSecure
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : NinjectHttpApplication
    {
        /// <summary>
        /// Register Global Filters
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
#if (!DEBUG)
            filters.Add(new RequireHttpsAttribute());
#endif
            filters.Add(new HandleErrorAttribute { View = "Error" });
        }

        /// <summary>
        /// Register Routes
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        /// <summary>
        /// Create Kernel
        /// </summary>
        /// <returns></returns>
        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        /// <summary>
        /// On Application Started
        /// </summary>
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            //we don't need validation on types that we don't specifically specify
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            //Register the binder used for decimals.
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            AutoMapperHelper.SetUp();
        }
    }
}