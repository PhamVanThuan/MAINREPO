using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Query.Models.Core
{
    public interface IRelationshipDefinition
    {
        string RelatedEntity { get; set; }
        Type DataModelType { get; set; }
        RelationshipType RelationshipType { get; set; }
        List<IRelatedField> RelatedFields { get; set; }

        string GetName();
    }
}