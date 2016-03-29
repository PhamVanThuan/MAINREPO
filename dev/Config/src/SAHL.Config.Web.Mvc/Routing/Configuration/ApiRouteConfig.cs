using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public class ApiRouteConfig
    {
        private readonly IRouteRetriever routeRetriever;

        public ApiRouteConfig(IRouteRetriever routeRetriever)
        {
            this.routeRetriever = routeRetriever;
        }

        public virtual void RegisterRoutes(HttpRouteCollection routes, IEnumerable<ICustomRoute> customRoutes)
        {
            if (customRoutes == null)
            {
                AddDefaultRoute(routes);
                return;
            }

            var transform = new LongestNonParameterisedFirstOrdering();
            var customApiRoutes = this.routeRetriever.GetRoutes<CustomApiRoute>(customRoutes, transform);

            if (customApiRoutes.Any())
            {
                AddCustomRoutes(routes, customApiRoutes);
            }
            AddDefaultRoute(routes);
        }

        private void AddDefaultRoute(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
        }

        private void AddCustomRoutes(HttpRouteCollection routes, IEnumerable<ICustomRoute> customRoutes)
        {
            foreach (var item in customRoutes)
            {
                routes.MapHttpRoute(item.Name, item.Url, item.Defaults, item.Constraints);
            }
        }
    }
}
