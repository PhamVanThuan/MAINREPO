using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Services.Query.Models.ThirdParty
{
    public class ThirdPartyPaymentBankAccountDataModel : IQueryDataModel
    {
        public ThirdPartyPaymentBankAccountDataModel()
        {
            SetupRelationships();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
        public int? ThirdPartyKey { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public int? BankAccountKey { get; set; }
        public string AccountName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public int? GeneralStatusKey { get; set; }
        public string BeneficiaryBankCode { get; set; } 

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }

    }
}
