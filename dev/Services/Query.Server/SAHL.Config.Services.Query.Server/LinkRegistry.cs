using SAHL.Services.Query;
using SAHL.Services.Query.Metadata;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class LinkRegistry : Registry
    {
        public LinkRegistry()
        {
            var representationDataModelScanner = new RepresentationDataModelScanner("SAHL.Services.Query");
            var dataModelTypeMaps = representationDataModelScanner.GetMappings();

            var container = new Container(new RouteMetadataRegistry());
            var linkItems = container.GetInstance<IEnumerable<LinkMetadata>>(RouteMetadataRegistry.RouteMetadataInstanceName);

            var routeModelMapping = new Dictionary<string, Type>();
            foreach (var item in linkItems)
            {
                var dataModelType = dataModelTypeMaps.SingleOrDefault(a => a.Key == item.RepresentationType);

                var url = item.UrlTemplate.StartsWith("~/") && item.UrlTemplate.Length > 2
                    ? item.UrlTemplate.Substring(2)
                    : item.UrlTemplate;

                routeModelMapping.Add(url, dataModelType.Value);
            }

            this.For<IRouteMetadataCollection>()
                .Singleton()
                .Use<RouteMetadataCollection>()
                .Ctor<IDictionary<string, Type>>()
                .Is(routeModelMapping);

            var items = linkItems
                .GroupBy(a => a.Name)
                .ToDictionary(a => a.Key, CreateRelationshipDictionary, StringComparer.OrdinalIgnoreCase);

            this.For<ILinkMetadataCollection>()
                .Singleton()
                .Use<LinkMetadataCollection>()
                .Ctor<IDictionary<string, IDictionary<string, IList<LinkMetadata>>>>()
                .Is(items);

            this.For<ILinkResolver>()
                .Singleton()
                .Use<LinkResolver>()
                .Ctor<ConcurrentDictionary<Type, Link>>()
                .Is(new ConcurrentDictionary<Type, Link>());
        }

        private static IDictionary<string, IList<LinkMetadata>> CreateRelationshipDictionary(
            IGrouping<string, LinkMetadata> itemsGroupedByControllerName)
        {
            return itemsGroupedByControllerName
                .GroupBy(a => a.Relationship)
                .ToDictionary(a => a.Key, b => (IList<LinkMetadata>)b.Select(c => c).ToList(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
