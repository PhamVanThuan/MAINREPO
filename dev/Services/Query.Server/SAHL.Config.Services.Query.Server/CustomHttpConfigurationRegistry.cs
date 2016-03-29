using SAHL.Config.Web.Api.Documentation;
using SAHL.Config.Web.Mvc;
using SAHL.Config.Web.Mvc.MediaTypeFormatting.Configuration;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;

namespace SAHL.Config.Services.Query.Server
{
    public class CustomHttpConfigurationRegistry : Registry
    {
        public CustomHttpConfigurationRegistry()
        {
            var container = new Container(new CustomRouteRegistry());
            var customRoutes = container.GetInstance<IEnumerable<ICustomRoute>>(CustomRouteRegistry.CustomRoutesInstanceName);

            this.For<RouteRegistration>()
                .Use<RouteRegistration>()
                .Ctor<IEnumerable<ICustomRoute>>()
                .Is(customRoutes);

            var mediaTypeFormatterContainer = new Container(new MediaTypeFormatterRegistry());
            var mediaTypeFormatters = mediaTypeFormatterContainer
                .GetInstance<IDictionary<Type, MediaTypeFormatter>>(MediaTypeFormatterRegistry.MediaTypeFormatterInstanceName)
                .Select(a => a.Value);

            var routeContainer = new Container(new RouteRegistry());
            var routeRetriever = routeContainer.GetInstance<IRouteRetriever>();

            var registrations = new List<IRegistrable>
            {
                new SwaggerApiDocumentationRegistrable("Query Service","SAHL.Services.Query"),
                new RouteRegistration(new ApiRouteConfig(routeRetriever), new MvcRouteConfig(routeRetriever), customRoutes),
                new MediaTypeFormatterRegistration(mediaTypeFormatters),
            };

            this.For<CustomHttpConfiguration>()
                .Singleton()
                .Use(new CustomHttpConfiguration(registrations));
        }
    }
}