using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace SAHL.Tools.RestServiceRoutenator
{
    public class Routes
    {
        private Dictionary<string, Routes> route = new Dictionary<string, Routes>();

        public Routes(string routePartName, int level, string qualifiedRoute)
        {
            RoutePart = routePartName;
            Level = level;
            QualifiedRoute = qualifiedRoute;
        }

        public string QualifiedRoute { get; set; }

        public int Level { get; set; }

        public IEnumerable<string> ChildRoutePartNames { get { return route.Keys; } }

        public IEnumerable<Routes> ChildRoutes { get { return route.Values; } }

        public string RoutePart { get; private set; }

        public bool Valid { get; set; }

        public Routes AddAndGet(string key)
        {
            if (LooksLikeAnId(key) )
            {
                key = "{Id}";
            }

            if (!route.ContainsKey(key))
                route.Add(key, new Routes(key, Level + 1, string.Format("{0}/{1}", QualifiedRoute, key.StartsWith("{")?  GetParameterPlaceHolder :key ).Replace("//", "/")));

            return route[key];
        }

        private string GetParameterPlaceHolder { get { return string.Format("[{0}]", QualifiedRoute.Count(x => x == '[')); } }

        private static bool LooksLikeAnId(string key)
        {
            return key.EndsWith("id}", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsParameter(string key)
        {
            return key.StartsWith("{");
        }

        public Routes this[string token]
        {
            get { return route[token]; }
        }
    }
}