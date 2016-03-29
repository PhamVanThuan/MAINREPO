using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Routing;
using NSubstitute;
using SAHL.Config.Services;
using SAHL.Config.Web.Mvc;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Controllers.Lookup;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Lookup;
using SAHL.Services.Query.Server.Specs.Helpers;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.Routing
{
    public class routing_a_request_for_generic_lookup : WithFakes
    {
        Establish that = () =>
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            configuration = An<HttpConfiguration>();

            var customConfiguration = container.GetInstance<CustomHttpConfiguration>();

            var registration = (RouteRegistration)customConfiguration.Registrations.Single(a => a.GetType() == typeof(RouteRegistration));

            customRoutes = registration.CustomRoutes.ToList();

            routeRetriever = An<IRouteRetriever>();

            routeRetriever
                .WhenToldTo(a => a.GetRoutes<CustomApiRoute>(customRoutes, Arg.Any<ICollectionTransform<ICustomRoute>>()))
                .Return(customRoutes.Cast<CustomApiRoute>());

            var routeRegistration = new RouteRegistration(new ApiRouteConfig(routeRetriever), new MvcRouteConfig(routeRetriever), customRoutes, configuration.Routes, new RouteCollection());
            routeRegistration.Register();

            httpMethod = HttpMethod.Get;
            lookUpType = "MyLookupType";
            requestMessage = new HttpRequestMessage(httpMethod, "http://localhost/api/Lookup/" + lookUpType);
        };

        private Because of = () =>
        {
            routeData = configuration.Routes.GetRouteData(requestMessage);

            //need to store for assertion
            requestMessage.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controllerContext = new HttpControllerContext(configuration, routeData, requestMessage);

            controllerSelector = new DefaultHttpControllerSelector(configuration);
            controllerDescriptor = controllerSelector.SelectController(requestMessage);
            controllerContext.ControllerDescriptor = controllerDescriptor;

            actionSelector = new ApiControllerActionSelector();
            actionMappings = actionSelector.GetActionMapping(controllerDescriptor);
            actionDescriptor = actionSelector.SelectAction(controllerContext);
        };

        private It should_have_chosen_the_generic_lookup_controller = () =>
        {
            controllerDescriptor.ControllerType.ShouldEqual(typeof(LookupController));
        };

        private It should_have_chosen_the_get_action = () =>
        {
            actionDescriptor.ActionName.ShouldEqual("Get");
        };

        private It should_have_a_controller_method_with_matching_parameters = () =>
        {
            var controllerMethodInfo = TestHelpers.GetMethodInfo((LookupController controller) => controller.Get(lookUpType));
            var controllerParameters = controllerMethodInfo
                .GetParameters()
                .Select(a => new Tuple<string, Type>(a.Name, a.ParameterType))
                .ToList();

            var actionParameters = actionDescriptor
                .GetParameters()
                .Select(a => new Tuple<string, Type>(a.ParameterName, a.ParameterType))
                .ToList();

            controllerParameters.ShouldEqual(actionParameters);
        };

        private static IHttpRouteData routeData;
        private static HttpConfiguration configuration;
        private static HttpRequestMessage requestMessage;
        private static HttpControllerContext controllerContext;
        private static HttpControllerDescriptor controllerDescriptor;
        private static HttpActionDescriptor actionDescriptor;
        private static ApiControllerActionSelector actionSelector;
        private static DefaultHttpControllerSelector controllerSelector;
        private static string lookUpType;
        private static HttpMethod httpMethod;
        private static ILookup<string, HttpActionDescriptor> actionMappings;
        private static List<ICustomRoute> customRoutes;
        private static IRouteRetriever routeRetriever;
    }
}
