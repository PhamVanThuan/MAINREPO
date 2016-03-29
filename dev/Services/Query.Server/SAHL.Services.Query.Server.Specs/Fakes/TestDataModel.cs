using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models.Core;

namespace SAHL.Services.Query.Server.Specs.Fakes
{
    public class TestDataModel : IQueryDataModel
    {
        public TestDataModel()
        {
            this.AliaseLookup = new Dictionary<string, string>();
            this.AliaseLookup.Add("Id", "T.Id");
            this.AliaseLookup.Add("Description", "T.Description");
            this.AliaseLookup.Add("Name", "Name");

            this.Relationships = new List<IRelationshipDefinition>();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }

        public int? Id { get; set; }
        public string Description { get; set; }
    }
}