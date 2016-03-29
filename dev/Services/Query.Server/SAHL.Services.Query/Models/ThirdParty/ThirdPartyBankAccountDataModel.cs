using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Services.Query.Models.ThirdParty
{
    public class ThirdPartyBankAccountDataModel : IQueryDataModel
    {
        public ThirdPartyBankAccountDataModel()
        {
            SetupRelationships();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public int? ThirdPartyKey { get; set; }
        public int? LegalEntityKey { get; set; }
        public int? BankAccountKey { get; set; }
        public string AccountNumber { get; set; }
        public string BranchCode { get; set; }
        public string AccountName { get; set; }
        public string BankingInstitute { get; set; }


        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }

    }
}
