using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Query.Builders.Core;

namespace SAHL.Services.Query.Metadata
{
    public class RouteMetadataCollection : IRouteMetadataCollection
    {
        private readonly IDictionary<string, Type> routes;

        public RouteMetadataCollection(IDictionary<string, Type> routes)
        {
            this.routes = routes;
        }

        public Type Get(string route)
        {
            Type result;
            routes.TryGetValue(route, out result);
            return result;
        }
    }

    public interface IRouteMetadataCollection
    {
        Type Get(string route);
    }

    public class LinkMetadataCollection : ILinkMetadataCollection
    {
        private readonly IDictionary<string, IDictionary<string, IList<LinkMetadata>>> links;

        public LinkMetadataCollection(IDictionary<string, IDictionary<string, IList<LinkMetadata>>> links)
        {
            this.links = links;
        }

        public IDictionary<string, IList<LinkMetadata>> this[string entityName]
        {
            get
            {
                IDictionary<string, IList<LinkMetadata>> result;
                this.links.TryGetValue(entityName, out result);
                return result;
            }
        }

        public string GetEntityName(Type representationType)
        {
            return representationType.Name.Replace("Representation", string.Empty);
        }

        public LinkMetadata this[string entityName, string relationshipName, string linkName = null]
        {
            get
            {
                var entityLinks = this[entityName];
                if (entityLinks == null)
                {
                    return null;
                }
                IList<LinkMetadata> result;
                entityLinks.TryGetValue(relationshipName, out result);
                if (result == null)
                {
                    return null;
                }
                if (result.Count <= 1)
                {
                    return linkName == null
                        ? result.Single(a => a.Name.Equals("self", StringComparison.OrdinalIgnoreCase)
                            || (entityName.Equals(relationshipName, StringComparison.OrdinalIgnoreCase) && a.Name.Equals(entityName)))
                        : result.Single(a => a.Name.Equals(linkName, StringComparison.OrdinalIgnoreCase));
                }

                //if there are multiple URLs to choose from, try and take the first one that isn't templated
                return result.FirstOrDefault(a => !a.UrlTemplate.Contains("{"))
                    ?? result.First();
            }
        }

        public LinkMetadata this[Type representationType, string relationshipName = null, string linkName = null]
        {
            get
            {
                var entityName = GetEntityName(representationType);
                return this[entityName, relationshipName ?? entityName, linkName];
            }
        }

        public LinkMetadata this[Type representationType, Type representationTypeRelatedTo, string linkName = null]
        {
            get
            {
                var entityName = GetEntityName(representationType);
                var relatedEntityName = GetEntityName(representationTypeRelatedTo);

                return this[entityName, relatedEntityName, linkName];
            }
        }

        public IEnumerator<KeyValuePair<string, IDictionary<string, IList<LinkMetadata>>>> GetEnumerator()
        {
            return this.links.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
