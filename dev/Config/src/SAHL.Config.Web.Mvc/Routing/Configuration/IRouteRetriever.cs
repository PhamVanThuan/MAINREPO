using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public interface IRouteRetriever
    {
        IEnumerable<ICustomRoute> GetRoutes<T>(IEnumerable<ICustomRoute> customRoutes, ICollectionTransform<ICustomRoute> routeCollectionTransform)
            where T : ICustomRoute;
    }
}
