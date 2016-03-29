using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Config.Web.Mvc.Routing;

namespace SAHL.Config.Web.Mvc.Specs.Routing.Fakes
{
    public class CustomRouteForTesting : CustomRoute
    {
        public CustomRouteForTesting(string name, string url, object defaults = null, object constraints = null) 
            : base(name, url, defaults, constraints)
        {
        }
    }
}
