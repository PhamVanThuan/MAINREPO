using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Web.Mvc
{
    public class RouteRegistry : Registry
    {
        public RouteRegistry()
        {
            this.For<IRouteRetriever>()
                .Singleton()
                .Use<RouteRetriever>();
        }
    }
}
