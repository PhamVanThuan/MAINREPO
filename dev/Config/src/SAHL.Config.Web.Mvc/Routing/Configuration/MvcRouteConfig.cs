using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public class MvcRouteConfig
    {
        private readonly IRouteRetriever routeRetriever;

        public MvcRouteConfig(IRouteRetriever routeRetriever)
        {
            this.routeRetriever = routeRetriever;
        }

        public virtual void RegisterRoutes(RouteCollection routes, IEnumerable<ICustomRoute> customRoutes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            if (customRoutes == null)
            {
                AddDefaultRoute(routes);
                return;
            }

            var transform = new LongestNonParameterisedFirstOrdering();
            var customMvcRoutes = this.routeRetriever.GetRoutes<CustomMvcRoute>(customRoutes, transform);

            if (customMvcRoutes.Any())
            {
                AddCustomRoutes(routes, customMvcRoutes);
            }
            AddDefaultRoute(routes);
        }

        private static void AddDefaultRoute(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Metrics", action = "Index", id = UrlParameter.Optional }
                );
        }

        private void AddCustomRoutes(RouteCollection routes, IEnumerable<ICustomRoute> customRoutes)
        {
            foreach (var item in customRoutes)
            {
                routes.MapRoute(item.Name, item.Url, item.Defaults, item.Constraints);
            }
        }
    }
}
