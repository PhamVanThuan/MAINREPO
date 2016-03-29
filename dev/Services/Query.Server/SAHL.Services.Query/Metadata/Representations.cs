using System;
using StructureMap;
using WebApi.Hal;

namespace SAHL.Services.Query.Metadata
{
    public static class Representations
    {
        private static readonly ILinkMetadataCollection linkMetadata;

        static Representations()
        {
            linkMetadata = ObjectFactory.GetInstance<ILinkMetadataCollection>();
        }

        [Obsolete("Inject an instance of ILinkMetadataCollection. Alternatively, inject a ILinkResolver, and retrieve links via the GetLink() method.")]
        public static ILinkMetadataCollection LinkMetadata
        {
            get { return linkMetadata; }
        }

        [Obsolete("Inject an instance of ILinkMetadataCollection.")]
        public static LinkMetadata GetLinkMetadata(this Representation representation)
        {
            return LinkMetadata[representation.GetType()];
        }
    }
}