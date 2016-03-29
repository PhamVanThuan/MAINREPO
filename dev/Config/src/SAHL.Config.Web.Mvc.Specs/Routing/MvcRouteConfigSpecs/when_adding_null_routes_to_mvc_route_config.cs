using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Web.Mvc.Routing.Configuration;

namespace SAHL.Config.Web.Mvc.Specs.MvcRouteConfigSpecs
{
    public class when_adding_null_routes_to_mvc_route_config : WithFakes
    {
        private const string defaultIgnoreRouteUrl = "{resource}.axd/{*pathInfo}";

        Establish that = () =>
        {
            routeCollection = new RouteCollection();
            routeRetriever = An<IRouteRetriever>();
            configuration = new MvcRouteConfig(routeRetriever);
        };

        private Because of = () =>
        {
            configuration.RegisterRoutes(routeCollection, null);
        };

        private It should_have_had_a_route_added_to_the_collection = () =>
        {
            routeCollection.Cast<Route>().Count(a => !a.Url.Equals(defaultIgnoreRouteUrl)).ShouldEqual(1);
        };

        private It should_have_the_default_ignore_route = () =>
        {
            routeCollection.Cast<Route>().Count(a => a.Url.Equals(defaultIgnoreRouteUrl)).ShouldEqual(1);
        };

        private It should_have_the_ignore_route_listed_first = () =>
        {
            routeCollection.Cast<Route>().First().Url.ShouldEqual(defaultIgnoreRouteUrl);
        };

        private It should_have_used_the_default_routing_url = () =>
        {
            routeCollection.Cast<Route>().Last().Url.ShouldEqual("{controller}/{action}/{id}");
        };

        private It should_have_the_id_as_a_default = () =>
        {
            routeCollection.Cast<Route>().Last().Defaults["id"].ShouldEqual(UrlParameter.Optional);
        };

        private It should_have_a_controller_as_a_default = () =>
        {
            routeCollection.Cast<Route>().Last().Defaults["controller"].ShouldEqual("Metrics");
        };

        private It should_have_an_action_as_a_default = () =>
        {
            routeCollection.Cast<Route>().Last().Defaults["action"].ShouldEqual("Index");
        };

        private It should_have_no_constraints = () =>
        {
            routeCollection.Cast<Route>().Last().Constraints.ShouldBeEmpty();
        };

        private static MvcRouteConfig configuration;
        private static RouteCollection routeCollection;
        private static IRouteRetriever routeRetriever;
    }
}
