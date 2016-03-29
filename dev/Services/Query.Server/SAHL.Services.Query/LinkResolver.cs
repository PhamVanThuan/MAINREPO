using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Core.Extensions;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query
{
    public class LinkResolver : ILinkResolver
    {
        private ILinkMetadataCollection LinkMetadata { get; set; }
        private ConcurrentDictionary<Type, Link> Links { get; set; }

        public LinkResolver(ConcurrentDictionary<Type, Link> links, ILinkMetadataCollection linkMetadata)
        {
            this.Links = links;
            this.LinkMetadata = linkMetadata;
        }

        public Link GetLink(string name)
        {
            return this.LinkMetadata[name].Single().Value.Single().ToHalLink();
        }

        public Link GetLink(Type representationType)
        {
            return GetLink(representationType, false);
        }
        
        public Link GetLink(Type representationType, bool isSelf)
        {
            return this.Links.TryGetValueIfNotPresentThenAdd(representationType, a => Representations.LinkMetadata[representationType].ToHalLink());
        }
        
        private Link GetLinkFromSelfDiscoveredLinks(Type representationType, bool isSelf)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("To retrieve a link for a representation type, the current HttpContext cannot be null");
            }

            var currentRequest = HttpContext.Current.Request;

            var link = HttpUtility.UrlDecode(currentRequest.Url.AbsolutePath);

            if (isSelf)
            {
                return new Link("self", link);
            }

            return CreateLink(representationType, link);
        }

        private static Link CreateLink(Type representationType, string link)
        {
            var rel = representationType.Name
                .Replace("ListRepresentation", string.Empty)
                .Replace("Representation", string.Empty)
                .ToCamelCase();

            //TODO: should have a way to resolve these without explicit checks
            if (rel.Equals("Root", StringComparison.OrdinalIgnoreCase))
            {
                return new Link(rel, "~/api/");
            }

            var linkWithRelationship = CombineRelWithLink(link, rel);

            return new Link(rel, linkWithRelationship);
        }

        private static string CombineRelWithLink(string link, string rel)
        {
            if (link.EndsWith(rel))
            {
                return link;
            }
            string linkWithRelationship;
            if (link.EndsWith("/"))
            {
                linkWithRelationship = link + rel;
            }
            else
            {
                linkWithRelationship = link + "/" + rel;
            }
            return linkWithRelationship;
        }

        public Link GetLink(Representation representation)
        {
            var type = representation.GetType();
            return GetLink(type);
        }

        public Link GetLink(Type representationType, params object[] parameters)
        {
            return GetLink(representationType).CreateLink(parameters);
        }

        public Link GetLink(Representation representation, params object[] parameters)
        {
            return GetLink(representation).CreateLink(parameters);
        }

        public string GetHref(Representation representation)
        {
            return GetLink(representation).Href;
        }

        public string GetHref(Representation representation, params object[] parameters)
        {
            return GetLink(representation, parameters).Href;
        }

        public string GetHref(Type representationType)
        {
            return GetLink(representationType).Href;
        }

        public string GetHref(Type representationType, params object[] parameters)
        {
            return GetLink(representationType, parameters).Href;
        }

        public string GetRel(Representation representation)
        {
            return GetLink(representation).Rel;
        }

        public string GetRel(Type representationType)
        {
            return GetLink(representationType).Rel;
        }
        
    }
}
