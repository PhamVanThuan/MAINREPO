using System.Collections.Generic;
using System.Linq;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public class RootAndFirstChildrenFilter : ICollectionTransform<string>
    {
        public IEnumerable<string> Transform(IEnumerable<string> source)
        {
            var groupedRoutes = source
                .Where(a => !a.Contains("{"))
                .GroupBy(a => a.Count(b => b == '/'));

            var minKey = groupedRoutes.Min(a => a.Key);

            return groupedRoutes
                .Where(a => a.Key == minKey) //return the shortest roots only
                .SelectMany(a => a);
        }
    }
}