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
    public class when_ordering_routes_with_shortest_path_first_ordering : WithFakes
    {
        Establish that = () =>
        {
            route1 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/rainbow");
            route2 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/{a}");
            route3 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/rainbowWay");
            route4 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/{a}/{b}");

            route5 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/rainbow/way/up/high");
            route6 = new CustomApiRoute(Guid.NewGuid().ToString(), "/somewhere/over/the/{a}/{b}/{c}/{d}");

            routes = new List<ICustomRoute>
            {
                route1,
                route2,
                route3,
                route4,
                route5,
                route6,
            };

            transform = new RootAndFirstChildrenFilter();
        };

        private Because of = () =>
        {
            result = transform.Transform(routes.Select(a => a.Url)).ToList();
        };

        private It should_have_the_expected_order = () =>
        {
            result.ShouldBeLike(new List<string>
            {
                route1.Url,
                route3.Url,
            });
        };

        private It should_not_have_any_templated_urls = () =>
        {
            result.Where(a => a.Contains("{")).ShouldBeEmpty();
        };

        private static ICollectionTransform<string> transform;
        private static List<ICustomRoute> routes;
        private static List<string> result;
        private static CustomApiRoute route1;
        private static CustomApiRoute route2;
        private static CustomApiRoute route3;
        private static CustomApiRoute route4;
        private static CustomApiRoute route5;
        private static CustomApiRoute route6;
    }
}
