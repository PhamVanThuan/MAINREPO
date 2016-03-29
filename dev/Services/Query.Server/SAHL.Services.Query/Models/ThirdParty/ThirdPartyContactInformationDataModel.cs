using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.Models.ThirdParty
{
    public class ThirdPartyContactInformationDataModel : IQueryDataModel
    {
        public ThirdPartyContactInformationDataModel()
        {
            SetupRelationships();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }
        
        public Guid Id { get; set; }
        public int? LegalEntityKey { get; set; }
        public string HomePhoneNumber { get; set; } 
        public string WorkPhoneNumber { get; set; } 
        public string FaxNumber { get; set; } 
        public string CellPhoneNumber { get; set; } 
        public string PostalAddress { get; set; } 
        public string ResidentialAddress { get; set; } 
        public string EmailAddress { get; set; }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }

    }
}