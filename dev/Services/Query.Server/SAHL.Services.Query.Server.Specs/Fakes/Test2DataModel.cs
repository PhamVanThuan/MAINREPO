using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Server.Specs.Fakes
{
    public class Test2DataModel : IQueryDataModel
    {
        public int? Id { get; set; }
        public string Value { get; set; }
        public int? TestId { get; set; }

        public List<IRelationshipDefinition> Relationships { get; set; }
    }
}