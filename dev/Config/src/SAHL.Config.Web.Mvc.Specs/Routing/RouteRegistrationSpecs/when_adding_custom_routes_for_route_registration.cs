using System.Collections.Generic;
using System.Web.Http;
using System.Web.Routing;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using SAHL.Core;

namespace SAHL.Config.Web.Mvc.Specs.RouteRegistrationSpecs
{
    public class when_adding_custom_routes_for_route_registration : WithFakes
    {
        Establish that = () =>
        {
            container = An<IIocContainer>();

            customRoutes = new List<ICustomRoute>
            {
                new CustomApiRoute(null, null)
            };

            routeRetriever = An<IRouteRetriever>();

            apiRouteConfig = An<ApiRouteConfig>(routeRetriever);
            mvcRouteConfig = An<MvcRouteConfig>(routeRetriever);
            
            apiRouteCollection = An<HttpRouteCollection>();
            mvcRouteCollection = An<RouteCollection>();

            routeRegistration = new RouteRegistration(apiRouteConfig, mvcRouteConfig, customRoutes, apiRouteCollection, mvcRouteCollection);
        };

        private Because of = () =>
        {
            routeRegistration.Register();
        };

        private It should_have_provided_the_retrieved_custom_routes_to_the_api_config = () =>
        {
            apiRouteConfig
                .WasToldTo(a => a.RegisterRoutes(Param.IsAny<HttpRouteCollection>(), customRoutes))
                .OnlyOnce();
        };

        private It should_have_provided_the_retrieved_custom_routes_to_the_mvc_config = () =>
        {
            mvcRouteConfig
                .WasToldTo(a => a.RegisterRoutes(Param.IsAny<RouteCollection>(), customRoutes))
                .OnlyOnce();
        };

        private static IIocContainer container;
        private static RouteRegistration routeRegistration;
        private static ApiRouteConfig apiRouteConfig;
        private static MvcRouteConfig mvcRouteConfig;
        private static List<ICustomRoute> customRoutes;
        private static HttpRouteCollection apiRouteCollection;
        private static RouteCollection mvcRouteCollection;
        private static IRouteRetriever routeRetriever;
    }
}
