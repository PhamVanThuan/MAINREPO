using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;

namespace SAHL.Config.Web.Mvc.Specs.ApiRouteConfigSpecs
{
    public class when_adding_custom_routes_to_api_route_config : WithFakes
    {
        Establish that = () =>
        {
            httpRouteCollection = new HttpRouteCollection();
            routeRetriever = An<IRouteRetriever>();

            configuration = new ApiRouteConfig(routeRetriever);

            customRoutes = new List<ICustomRoute>
            {
                new CustomApiRoute("1", "A", new { id = RouteParameter.Optional }, new { namespaces = "Somewhere" }),
                new CustomApiRoute("2", "B", new { id = RouteParameter.Optional, controller = "Banana" }, new { namespaces = "Somewhere.Else" }),
            };

            routeRetriever
                .WhenToldTo(a => a.GetRoutes<CustomApiRoute>(customRoutes, Arg.Any<ICollectionTransform<ICustomRoute>>()))
                .Return(customRoutes.Cast<CustomApiRoute>());
        };

        private Because of = () =>
        {
            configuration.RegisterRoutes(httpRouteCollection, customRoutes);
        };

        private It should_have_added_the_expected_number_of_custom_routes = () =>
        {
            //plus 1 for the existing default route
            httpRouteCollection.Count.ShouldEqual(customRoutes.Count + 1);
        };

        private It should_have_added_a_default_route = () =>
        {
            httpRouteCollection.ContainsKey("DefaultApi").ShouldBeTrue();
        };

        private It should_have_added_the_expected_custom_routes = () =>
        {
            for(var i = 0; i < customRoutes.Count; i++)
            {
                var httpRoute = httpRouteCollection[i];
                var customRoute = customRoutes[i];

                httpRouteCollection.ContainsKey(customRoute.Name).ShouldBeTrue();
                httpRoute.RouteTemplate.ShouldEqual(customRoute.Url);

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
            httpRouteCollection[1].Defaults["controller"].ShouldEqual(defaultController);
        };

        private static ApiRouteConfig configuration;
        private static HttpRouteCollection httpRouteCollection;
        private static List<ICustomRoute> customRoutes;
        private static IRouteRetriever routeRetriever;
    }
}
