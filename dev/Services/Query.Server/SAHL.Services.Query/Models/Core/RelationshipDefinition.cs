using System;
using System.Collections.Generic;
using SAHL.Core.Extensions;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Models.Core
{
    public class RelationshipDefinition : IRelationshipDefinition
    {
        public string RelatedEntity { get; set; }
        public Type DataModelType { get; set; }
        public RelationshipType RelationshipType { get; set; }
        public List<IRelatedField> RelatedFields { get; set; }

        public override int GetHashCode()
        {
            return HashCodeExtensions.GetHashCode(this.RelatedEntity, this.DataModelType, this.RelationshipType);
        }

        public string GetName()
        {
            return RelatedEntity ?? "null";
        }

        public override string ToString()
        {
            return GetName();
        }
    }
}
