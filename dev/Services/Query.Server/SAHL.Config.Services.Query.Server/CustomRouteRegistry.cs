using System;
using System.Net.Http;
using System.Web.Http.Routing;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Services.Query.Metadata;
using StructureMap;
using StructureMap.Configuration.DSL;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Config.Services.Query.Server
{
    public class CustomRouteRegistry : Registry
    {
        //TODO: add support for these later: HttpMethod.Post, HttpMethod.Put, HttpMethod.Delete, HttpMethod.Head, new HttpMethod("PATCH"), HttpMethod.Options
        private readonly HttpMethod[] httpMethods = { HttpMethod.Get };

        public const string CustomRoutesInstanceName = "QueryServiceCustomRoutesInstances";

        public CustomRouteRegistry()
        {
            var metadataContainer = new Container(new LinkRegistry());
            var linkCollection = metadataContainer.GetInstance<ILinkMetadataCollection>();

            var customRoutes = new List<ICustomRoute>();
            var linkItems = linkCollection
                .SelectMany(a => a.Value.Values)
                .SelectMany(a => a)
                ;

            foreach (var link in linkItems)
            {
                var routes = CreateRoutes(link);
                customRoutes.AddRange(routes);
            }

            this.For<IEnumerable<ICustomRoute>>()
                .Use(customRoutes)
                .Named(CustomRoutesInstanceName);
        }

        private IEnumerable<ICustomRoute> CreateRoutes(LinkMetadata link)
        {
            if (link.Action == null)
            {
                foreach (var method in this.httpMethods)
                {
                    yield return CreateCustomRoute(link, method);
                }
            }
            else
            {
                yield return CreateCustomRoute(link);
            }
        }

        private CustomApiRoute CreateCustomRoute(LinkMetadata link)
        {
            return CreateCustomRoute(link, FindHttpMethodForAction(link) ?? HttpMethod.Get);
        }

        private HttpMethod FindHttpMethodForAction(LinkMetadata link)
        {
            return this.httpMethods.FirstOrDefault(a => a.Method.Equals(link.Action, StringComparison.OrdinalIgnoreCase));
        }

        private static CustomApiRoute CreateCustomRoute(LinkMetadata link, HttpMethod method)
        {
            var defaults = new { controller = link.Controller, action = link.Action ?? method.Method };
            var constraints = new { httpMethod = new HttpMethodConstraint(method) };
            return new CustomApiRoute(link.Name, link.UrlTemplate, defaults, constraints);
        }
    }
}