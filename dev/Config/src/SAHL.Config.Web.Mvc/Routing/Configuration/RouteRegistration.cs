using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public class RouteRegistration : IRegistrable
    {
        private readonly HttpRouteCollection apiRouteCollection;
        private readonly ApiRouteConfig apiRouteConfig;
        private readonly RouteCollection mvcRouteCollection;
        private readonly MvcRouteConfig mvcRouteConfig;

        public RouteRegistration(ApiRouteConfig apiRouteConfig, MvcRouteConfig mvcRouteConfig, IEnumerable<ICustomRoute> customRoutes,
            HttpRouteCollection apiRouteCollection = null, RouteCollection mvcRouteCollection = null)
        {
            this.apiRouteConfig = apiRouteConfig;
            this.mvcRouteConfig = mvcRouteConfig;
            this.CustomRoutes = customRoutes ?? Enumerable.Empty<ICustomRoute>();
            this.apiRouteCollection = apiRouteCollection;
            this.mvcRouteCollection = mvcRouteCollection;
        }

        public IEnumerable<ICustomRoute> CustomRoutes { get; private set; }

        public void Register()
        {
            var duplicates = CustomRoutes.GroupBy(a => a.Name).Where(a => a.Count() > 1);

            var count = 0;
            foreach (var item in duplicates.SelectMany(a => a))
            {
                item.Name = item.Name + count++;
            }

            this.apiRouteConfig.RegisterRoutes(apiRouteCollection ?? GlobalConfiguration.Configuration.Routes, this.CustomRoutes);

            this.mvcRouteConfig.RegisterRoutes(mvcRouteCollection ?? RouteTable.Routes, this.CustomRoutes);
        }
    }
}
