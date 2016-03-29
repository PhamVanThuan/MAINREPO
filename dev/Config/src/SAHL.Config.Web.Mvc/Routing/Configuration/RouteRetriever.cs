using System.Collections.Generic;
using System.Linq;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public class RouteRetriever : IRouteRetriever
    {
        public IEnumerable<ICustomRoute> GetRoutes<T>(IEnumerable<ICustomRoute> customRoutes, ICollectionTransform<ICustomRoute> routeCollectionTransform = null)
            where T : ICustomRoute
        {
            var routes = customRoutes
                .Where(a => a.GetType() == typeof(T));

            return routeCollectionTransform == null
                ? routes
                : routeCollectionTransform.Transform(routes);
        }
    }
}
