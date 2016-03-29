using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Factories;

namespace SAHL.Services.Query.Server.Tests.Models
{
    public class TestDataModel : IQueryDataModel
    {

        public TestDataModel()
        {
            AliaseLookup = DictionaryFactory.CreateCaseInsensitiveDictionary();
            AliaseLookup.Add("Id", "T.Id");
            AliaseLookup.Add("Count", "T.Count");
            AliaseLookup.Add("Description", "T.Description");
            AliaseLookup.Add("Name", "Name");

            Relationships = new List<IRelationshipDefinition>();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
        public int? Count { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

    }


}