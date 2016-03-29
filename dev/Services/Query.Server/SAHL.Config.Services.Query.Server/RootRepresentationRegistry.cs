using SAHL.Config.Web.Mvc.Routing.Configuration;
using SAHL.Services.Query;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Controllers.Lookup;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Metadata;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class RootRepresentationRegistry : Registry
    {
        public const string RootRepresentationRegistryInstanceName = "RootRepresentationInstances";

        public RootRepresentationRegistry()
        {
            var routeContainer = new Container(new RouteMetadataRegistry());
            var rootItems = routeContainer.GetInstance<IEnumerable<LinkMetadata>>(RouteMetadataRegistry.RouteMetadataInstanceName);

            var linkItems = rootItems
                .Where(a => a.RepresentationType != typeof (RootRepresentation) //exclude link to itself (covered via self ref)
                    && typeof(Representation).IsAssignableFrom(a.RepresentationType) //should be a real representation
                    && a.RepresentationType.CustomAttributes //should not have been excluded from generation
                        .All(b => b.AttributeType != typeof(ServiceGenerationToolExcludeAttribute))
                );

            var transform = new RootAndFirstChildrenFilter();
            var items = transform.Transform(linkItems.Select(a => a.UrlTemplate));
            
            var container = new Container(new LinkRegistry());
            var linkMetadataCollection = container.GetInstance<ILinkMetadataCollection>();

            var rootRepresentationTypes = new List<Link>();
            foreach (var item in items)
            {
                var linkMetadata = linkItems.Single(a => a.UrlTemplate.Equals(item, StringComparison.OrdinalIgnoreCase));

                var globalLinkMetadata = linkMetadataCollection[linkMetadata.Name, linkMetadata.Name];

                var shouldTypeBePlural = typeof (IListRepresentation).IsAssignableFrom(globalLinkMetadata.RepresentationType);

                var rel = RemoveUnneededTokens(globalLinkMetadata.Relationship);

                rel = shouldTypeBePlural
                    ? Pluralise(rel)
                    : rel;

                rel = rel.ToCamelCase();

                var link = globalLinkMetadata.ToHalLink(rel);

                rootRepresentationTypes.Add(link);
            }

            this.For<IEnumerable<Link>>()
                .Singleton()
                .Use(rootRepresentationTypes)
                .Named(RootRepresentationRegistryInstanceName);
        }

        public string RemoveUnneededTokens(string relation)
        {
            //happens once at load up, stringbuilder doesn't have a 'case insensitive replace'
            var cleanedRelation = RemoveLastToken(relation, "_root");
            cleanedRelation = RemoveLastToken(cleanedRelation, "List");
            //do replacements here for the exceptions that aren't caught by the pluraliser

            return cleanedRelation;
        }

        private static string Pluralise(string relation)
        {
            var pluraliser = PluralizationService.CreateService(CultureInfo.CreateSpecificCulture("en-ZA"));
            return pluraliser.Pluralize(relation);
        }

        private static string RemoveLastToken(string source, string token)
        {
            var indexOfToken = source.LastIndexOf(token, StringComparison.OrdinalIgnoreCase);
            return indexOfToken >= 0
                ? PerformRemove(source, token, indexOfToken)
                : source;
        }

        private static string PerformRemove(string source, string token, int indexOfToken)
        {
            return source.Remove(indexOfToken, token.Length);
        }
    }
}