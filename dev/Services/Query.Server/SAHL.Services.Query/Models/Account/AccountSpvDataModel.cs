using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Models.Treasury;

namespace SAHL.Services.Query.Models.Account
{
    public class AccountSpvDataModel: IQueryDataModel
    {
        public AccountSpvDataModel()
        {
            SetupRelationships();
        }

        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
        public int? AccountKey { get; set; }
        public string SPVDescription { get; set; }
        public string ReportDescription { get; set; }
        public string CreditProviderNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public int? BankAccountKey { get; set; }
        public int? GeneralStatusKey { get; set; }
        public int? ParentSPVKey { get; set; }
        public string GeneralStatus { get; set; }        
        public string AccountName { get; set; }
        public string BranchCode { get; set; }        
        public string AccountNumber { get; set; }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "GeneralStatus",
                DataModelType = typeof(LookupDataModel),
                RelationshipType = RelationshipType.OneToOneLookup,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "GeneralStatusKey", RelatedKey = "GeneralStatusKey", Value = ""}
                    }
            });
        }
    }
}
