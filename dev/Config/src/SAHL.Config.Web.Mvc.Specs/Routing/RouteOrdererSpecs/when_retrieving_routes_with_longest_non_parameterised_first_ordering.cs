using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using SAHL.Core.Testing;

namespace SAHL.Config.Web.Mvc.Specs.Routing.RouteConfigBaseSpecs
{
    public class when_ordering_routes_with_longest_non_parameterised_first_ordering : WithFakes
    {
        Establish that = () =>
        {
            route3 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/rainbow/way/up/high");
            route1 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/rainbow");
            route4 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/{a}/{b}/{c}/{d}");
            route2 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/{a}");

            routes = new List<ICustomRoute>
            {
                route1,
                route2,
                route3,
                route4,
            };

            transform = new LongestNonParameterisedFirstOrdering();

        };

        private Because of = () =>
        {
            result = transform.Transform(routes).ToList();
        };

        private It should_have_the_expected_order = () =>
        {
            result.ShouldBeLike(new List<ICustomRoute>
            {
                route3,
                route1,
                route4,
                route2,
            });
        };

        private static ICollectionTransform<ICustomRoute> transform;
        private static List<ICustomRoute> routes;
        private static List<ICustomRoute> result;
        private static CustomApiRoute route1;
        private static CustomApiRoute route2;
        private static CustomApiRoute route3;
        private static CustomApiRoute route4;
    }
}
