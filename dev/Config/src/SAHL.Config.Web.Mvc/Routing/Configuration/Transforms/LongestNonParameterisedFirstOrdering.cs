using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public class LongestNonParameterisedFirstOrdering : ICollectionTransform<ICustomRoute>
    {
        public IEnumerable<ICustomRoute> Transform(IEnumerable<ICustomRoute> customRoutes)
        {
            return customRoutes
                .OrderBy(a => a.Url.Contains('{')) //false first -> non 'parameterised' first
                .ThenByDescending(a => a.Url.Count(b => b == '/'))  //longest resource path by forward slashes
                .ThenByDescending(a => a.Url.Split('/').Last().Length) //length of last token, longest first
                .ThenBy(a => a.Url)
                ;
        }
    }
}