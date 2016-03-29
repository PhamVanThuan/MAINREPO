using System;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Routing;
using NSubstitute;
using NUnit.Framework;
using SAHL.Config.Services;
using SAHL.Config.Web.Mvc;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using Machine.Fakes;
using Machine.Specifications;

namespace SAHL.Services.Query.Server.Specs.Routing
{
    //TODO: this should probably be a convention test
    public class ensure_all_routes_map_to_a_controller_and_a_get_action : WithFakes
    {
        Establish that = () =>
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            configuration = An<HttpConfiguration>();

            var customConfiguration = container.GetInstance<CustomHttpConfiguration>();

            var registration = (RouteRegistration)customConfiguration.Registrations.Single(a => a.GetType() == typeof(RouteRegistration));

            routeCollectionTransform = new LongestNonParameterisedFirstOrdering();
            
            customRoutes = routeCollectionTransform.Transform(registration.CustomRoutes).ToList();
            
            routeRetriever = An<IRouteRetriever>();

            routeRetriever
                .WhenToldTo(a => a.GetRoutes<CustomApiRoute>(customRoutes, Arg.Any<ICollectionTransform<ICustomRoute>>()))
                .Return(customRoutes.Cast<CustomApiRoute>());

            var routeRegistration = new RouteRegistration(new ApiRouteConfig(routeRetriever), new MvcRouteConfig(routeRetriever), customRoutes, configuration.Routes, new RouteCollection());
            routeRegistration.Register();

            httpMethod = HttpMethod.Get;

            customRouteParameters = new List<List<string>>();

            var tokens = customRoutes
                    .Select(a => a.Url
                        .Split(new[] { '/' })
                        .Where(b => b.StartsWith("{") && b.EndsWith("}"))
                        .Select(ReplaceVariablePlaceHoldersWithDummyValues)
                        .Select(b => b.ToLower())
                        .ToList()
                    );

            customRouteParameters.AddRange(tokens);

            routeExceptions = new List<Exception>();
        };

        private Because of = () =>
        {
            for(var i = 0; i < customRoutes.Count; i++)
            {
                var customRoute = customRoutes[i];
                var parameters = customRouteParameters[i];

                routeExceptions.Add(Catch.Exception(() => TryValidateRoute(customRoute, parameters)));
            }
        };

        private It should_have_no_invalid_routes = () =>
        {
            for (var i = 0; i < routeExceptions.Count; i++)
            {
                if (routeExceptions[i] == null)
                {
                    continue;
                }

                var message = string.Format("Route is invalid. Name: {0}, Template: {1}",
                    customRoutes[i].Name,
                    customRoutes[i].Url
                );

                throw new AssertionException(message, routeExceptions[i]);
            }
        };

        private static void TryValidateRoute(ICustomRoute customRoute, List<string> parameters)
        {
            var partialUrl = ReplaceVariablePlaceHoldersWithDummyValues(customRoute.Url);

            requestMessage = new HttpRequestMessage(httpMethod, "http://localhost/" + partialUrl);

            routeData = configuration.Routes.GetRouteData(requestMessage);

            if (routeData == null)
            {
                var message = string.Format("No routeData could be found for route {0} with url {1}", customRoute.Name, requestMessage.RequestUri);
                throw new AssertionException(message);
            }

            requestMessage.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controllerContext = new HttpControllerContext(configuration, routeData, requestMessage);

            controllerSelector = new DefaultHttpControllerSelector(configuration);

            SelectController(customRoute);
            SelectAction();
        }

        private static void SelectAction()
        {
            actionSelector = new ApiControllerActionSelector();
            actionSelector.GetActionMapping(controllerDescriptor);

            try
            {
                actionSelector.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                var message = string.Format("Could not find a {0} action on the {1} controller. See inner exception for details.",
                    requestMessage.Method.Method, controllerDescriptor.ControllerName);
                throw new AssertionException(message, ex);
            }
        }

        private static void SelectController(ICustomRoute customRoute)
        {
            try
            {
                controllerDescriptor = controllerSelector.SelectController(requestMessage);
            }
            catch (HttpResponseException ex)
            {
                ThrowControllerNotFound(ex);
            }

            if (controllerDescriptor == null)
            {
                ThrowControllerNotFound();
            }

            var controllerNameOnCustomRoute = (string)customRoute.Defaults.GetType().GetProperty("controller").GetValue(customRoute.Defaults);
            if (!controllerDescriptor.ControllerName.Equals(controllerNameOnCustomRoute, StringComparison.OrdinalIgnoreCase))
            {
                var message = string.Format(
                    "The custom route {0} is defined to resolve to the {1} controller, however the api routing has routed it to the {2} controller."
                        + " There may me a common prefix in the routes of the {1} controller and the {2} controller that is causing the mismatch."
                        + " NB: this is not a failure on the controller, but rather the route registration. See RouteRegistration class."
                    , customRoute.Url
                    , controllerNameOnCustomRoute
                    , controllerDescriptor.ControllerName
                    );
                throw new AssertionException(message);
            }

            controllerContext.ControllerDescriptor = controllerDescriptor;
        }

        private static void ThrowControllerNotFound(HttpResponseException ex = null)
        {
            var message = string.Format("Could not find a controller to select when routing {0}.{1}",
                requestMessage.RequestUri,
                ex == null ? string.Empty : " See inner exception for details.");
            throw new AssertionException(message, ex);
        }

        private static string ReplaceVariablePlaceHoldersWithDummyValues(string url)
        {
            return url
                .Replace("{", string.Empty)
                .Replace("}", string.Empty);
        }

        private static IHttpRouteData routeData;
        private static HttpConfiguration configuration;
        private static HttpRequestMessage requestMessage;
        private static HttpControllerContext controllerContext;
        private static HttpControllerDescriptor controllerDescriptor;
        private static DefaultHttpControllerSelector controllerSelector;
        private static ApiControllerActionSelector actionSelector;
        private static HttpMethod httpMethod;
        private static List<ICustomRoute> customRoutes;
        private static List<List<string>> customRouteParameters;
        private static List<Exception> routeExceptions;
        private static IRouteRetriever routeRetriever;
        private static ICollectionTransform<ICustomRoute> routeCollectionTransform;
    }
}
