using System.Linq;
using System.Web.Http;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;

namespace SAHL.Config.Web.Mvc.Specs.ApiRouteConfigSpecs
{
    public class when_adding_empty_routes_to_api_route_config : WithFakes
    {
        Establish that = () =>
        {
            httpRouteCollection = new HttpRouteCollection();
            routeRetriever = An<IRouteRetriever>();
            configuration = new ApiRouteConfig(routeRetriever);
        };

        private Because of = () =>
        {
            configuration.RegisterRoutes(httpRouteCollection, Enumerable.Empty<ICustomRoute>());
        };

        private It should_have_had_a_route_added_to_the_collection = () =>
        {
            httpRouteCollection.Count.ShouldEqual(1);
        };

        private It should_have_used_the_default_routing_template = () =>
        {
            httpRouteCollection.Single().RouteTemplate.ShouldEqual("api/{controller}/{id}");
        };

        private It should_only_have_a_single_default = () =>
        {
            httpRouteCollection.Single().Defaults.Any().ShouldBeTrue();
        };

        private It should_have_a_default_for_id = () =>
        {
            httpRouteCollection.Single().Defaults.Single().Key.ShouldEqual("id");
        };

        private It should_have_a_route_parameter_optional_as_the_default_value_for_id = () =>
        {
            httpRouteCollection.Single().Defaults.Single().Value.ShouldEqual(RouteParameter.Optional);
        };

        private static ApiRouteConfig configuration;
        private static HttpRouteCollection httpRouteCollection;
        private static IRouteRetriever routeRetriever;
    }
}
