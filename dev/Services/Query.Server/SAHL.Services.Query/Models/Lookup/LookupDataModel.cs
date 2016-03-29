using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Models.Lookup
{
    public class LookupDataModel : ILookupDataModel, IQueryDataModel
    {
        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }
    }
}