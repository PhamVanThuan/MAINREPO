using System;
using System.Collections.Generic;

namespace SAHL.Services.Query.Metadata
{
    public interface ILinkMetadataCollection : IEnumerable<KeyValuePair<string, IDictionary<string, IList<LinkMetadata>>>>
    {
        IDictionary<string, IList<LinkMetadata>> this[string entityName] { get; }

        LinkMetadata this[string entityName, string relationshipName, string linkName = null] { get; }
        LinkMetadata this[Type representationType, string relationshipName = null, string linkName = null] { get; }

        LinkMetadata this[Type representationType, Type representationTypeRelatedTo, string linkName = null] { get; }

        string GetEntityName(Type representationType);
    }
}