using System.Linq;
using System.Threading;

namespace SAHL.Tools.RestServiceRoutenator
{
    public class RouteFinder
    {
        private readonly ITokeniser tokeniser;
        private static Routes allRoutes = new Routes("", 0, "");

        public Routes AllRoutes
        {
            get { return allRoutes; }
        }

        public RouteFinder(ITokeniser tokeniser)
        {
            this.tokeniser = tokeniser;
        }

        public void Initialise(params string[] allRoutes)
        {
            foreach (var route in allRoutes.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var currentRoute = AllRoutes;
                var allTokensForRoute = tokeniser.TokeniseStringForRest(route);
                currentRoute = allTokensForRoute.Aggregate(currentRoute, (current, token) => current.AddAndGet(token));
                currentRoute.Valid = true;
            }
            RouteFinder.allRoutes = RouteFinder.allRoutes[""];
        }
    }
}