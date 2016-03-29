using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Interfaces.Query.Models
{
    public interface IQueryServiceCustomDataModel
    {
        List<IRelationshipDefinition> Relationships { get; set; }
    }
}