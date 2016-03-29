using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;

namespace SAHL.Config.Web.Mvc.Specs.MvcRouteConfigSpecs
{
    public class when_adding_custom_routes_to_mvc_route_config : WithFakes
    {
        private const string defaultIgnoreRouteUrl = "{resource}.axd/{*pathInfo}";

        Establish that = () =>
        {
            routeCollection = new RouteCollection();
            routeRetriever = An<IRouteRetriever>();
            configuration = new MvcRouteConfig(routeRetriever);

            customRoutes = new List<ICustomRoute>
            {
                new CustomMvcRoute("1", "A", new { id = RouteParameter.Optional }, new { namespaces = "Somewhere" }),
                new CustomMvcRoute("2", "B", new { id = RouteParameter.Optional, controller = "Banana" }, new { namespaces = "Somewhere.Else" }),
            };

            routeRetriever
                .WhenToldTo(a => a.GetRoutes<CustomMvcRoute>(customRoutes, Arg.Any<ICollectionTransform<ICustomRoute>>()))
                .Return(customRoutes.Cast<CustomMvcRoute>());
        };

        private Because of = () =>
        {
            configuration.RegisterRoutes(routeCollection, customRoutes);
        };

        private It should_have_added_the_expected_number_of_custom_routes = () =>
        {
            //+1 for the existing default route
            routeCollection
                .Cast<Route>()
                .Count(a => !a.Url.Equals(defaultIgnoreRouteUrl))
                .ShouldEqual(customRoutes.Count + 1);
        };

        private It should_have_added_the_default_route = () =>
        {
            routeCollection["Default"].ShouldNotBeNull();
        };

        private It should_have_added_the_expected_routes = () =>
        {
            var collection = routeCollection
                .Cast<Route>()
                .Where(a => !a.Url.Equals(defaultIgnoreRouteUrl))
                .ToList();

            for(var i = 0; i < customRoutes.Count; i++)
            {
                var httpRoute = collection[i];
                var customRoute = customRoutes[i];

                routeCollection[customRoute.Name].ShouldNotBeNull();
                httpRoute.Url.ShouldEqual(customRoute.Url);

                var defaults = httpRoute.Defaults;
                var defaultId = (object) ((dynamic) customRoute.Defaults).id;
                defaults["id"].ShouldEqual(defaultId);

                var constraints = httpRoute.Constraints;
                var constraintNamepsaces = (string)((dynamic) customRoute.Constraints).namespaces;
                constraints["namespaces"].ShouldEqual(constraintNamepsaces);
            }
        };

        private It should_have_added_the_expected_controller_default = () =>
        {
            var defaultController = (object) ((dynamic) customRoutes[1].Defaults).controller;
            ((Route)routeCollection[2]).Defaults["controller"].ShouldEqual(defaultController);
        };

        private static MvcRouteConfig configuration;
        private static RouteCollection routeCollection;
        private static List<ICustomRoute> customRoutes;
        private static IRouteRetriever routeRetriever;
    }
}
