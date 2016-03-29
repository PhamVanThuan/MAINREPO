using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Models.Process
{
    public class ProcessDataModel : IQueryDataModel
    {
        public ProcessDataModel()
        {
            SetupRelationships();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }

        public string Id { get; set; }
        public string Process { get; set; }
        public string Stage { get; set; }
        public string AccountKey { get; set; }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }
    }
}