using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Config.Web.Mvc.Routing;
using SAHL.Config.Web.Mvc.Routing.Configuration.Transforms;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Controllers.Attorney;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Core;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Query;

namespace SAHL.Config.Services.Query.Server
{
    public class RouteMetadataRegistry : Registry
    {
        public const string RouteMetadataInstanceName = "RouteMetadata";

        public RouteMetadataRegistry()
        {
            numberer = new CustomRouteDuplicateParameterNumberer();

            relatedFields = new HashSet<RelationshipDefinitionNavigation>(new RelationshipDefinitionNavigationEqualityComparer());
            relationshipChain = new Stack<IRelationshipDefinition>();

            var rootRepresentationAttributeScanner = new RootRepresentationAttributeScanner("SAHL.Services.Query");
            var rootItems = rootRepresentationAttributeScanner.GetLinks();

            var representationDataModelMapContainer = new Container(new RepresentationDataModelMapRegistry());
            var mappings = representationDataModelMapContainer.GetInstance<IRepresentationDataModelMapCollection>();

            var generatedLinks = ProcessMappings(mappings, rootItems);

            var scanner = new RepresentationRouteAttributeScanner("SAHL.Services.Query");
            var linkItems = scanner.GetLinks().Concat(generatedLinks);

            this.For<IEnumerable<LinkMetadata>>()
                .Singleton()
                .Use(linkItems)
                .Named(RouteMetadataInstanceName);
        }

        private List<LinkMetadata> ProcessMappings(IRepresentationDataModelMapCollection mappings, IEnumerable<LinkMetadata> rootItems)
        {
            var results = new List<LinkMetadata>();
            foreach (var item in rootItems)
            {
                this.counter = 0;
                this.relatedFields.Clear();
                this.relationshipChain.Clear();
                this.relationshipChain.Push(null);
                ProcessMapping(results, mappings, item);
            }
            return results;
        }

        private readonly HashSet<RelationshipDefinitionNavigation> relatedFields;
        private int counter;
        private readonly Stack<IRelationshipDefinition> relationshipChain;
        private readonly CustomRouteDuplicateParameterNumberer numberer;

        private void ProcessMapping(List<LinkMetadata> results, IRepresentationDataModelMapCollection mappings, LinkMetadata item)
        {
            //failsafe to ensure we don't explore relationships too deep: this is usually an error
            counter++;
            if (counter == 100)
            {
                return;
            }

            results.Add(item);

            var urlTemplate = item.UrlTemplate;
            var dataModelType = mappings.Get(item.RepresentationType);

            var type = dataModelType.IsGenericType && dataModelType.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                ? dataModelType.GenericTypeArguments[0]
                : dataModelType;

            if (!typeof(IQueryDataModel).IsAssignableFrom(type))
            {
                var message = string.Format(
                    "{0} is not an IQueryDataModel. Ensure that the relationship is defined on the parent of URL {1} correctly."
                    , type.Name
                    , item.UrlTemplate
                    );

                throw new InvalidOperationException(message);
            }

            if (IsListRepresentation(item.RepresentationType))
            {
                var instanceRepresentationType = mappings.Get(type);

                results.Add(new LinkMetadata(typeof(CountRepresentation), urlTemplate + "/count", item.Controller, action: "Count"));

                urlTemplate = urlTemplate + "/{id}";

                urlTemplate = numberer.Number(urlTemplate);

                results.Add(new LinkMetadata(instanceRepresentationType, urlTemplate, item.Controller, action: item.Action));
            }

            var modelInstance = (IQueryDataModel) Activator.CreateInstance(type);

            if (modelInstance.Relationships == null || !modelInstance.Relationships.Any())
            {
                return;
            }

            foreach (var relationship in modelInstance.Relationships)
            {
                var wasAdded = this.relatedFields.Add(new RelationshipDefinitionNavigation(this.relationshipChain.Peek(), relationship));
                if (!wasAdded)
                {
                    continue;
                }

                this.relationshipChain.Push(relationship);

                var representationType = mappings.Get(relationship.DataModelType);
                var relationshipLink = new LinkMetadata(representationType, urlTemplate + "/" + relationship.RelatedEntity, item.Controller,
                    action: item.Action);

                ProcessMapping(results, mappings, relationshipLink);

                this.relationshipChain.Pop();
            }
        }

        private static bool IsListRepresentation(Type representationType)
        {
            return typeof(IListRepresentation).IsAssignableFrom(representationType);
        }
    }
}