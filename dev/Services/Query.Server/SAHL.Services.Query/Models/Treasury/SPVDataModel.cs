using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.Models.Treasury
{
    public class SPVDataModel : IQueryDataModel
    {
        public SPVDataModel()
        {
            SetupRelationships();
        }

        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
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
