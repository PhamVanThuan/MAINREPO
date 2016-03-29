using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Models.Finance;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.Models.ThirdParty
{
    public class ThirdPartyDataModel : IQueryDataModel
    {

        public ThirdPartyDataModel()
        {
            SetupRelationships();
        }

        public Guid Id { get; set; }
        public int? ThirdPartyKey { get; set; }
        public int? LegalEntityKey { get; set; }
        public string LegalName { get; set; }
        public string RegistrationNumber { get; set; }
        public int? GeneralStatusKey { get; set; }
        public string GeneralStatusDescription { get; set; }
        public int? LegalEntityTypeKey { get; set; }
        public string LegaLEntityStatus { get; set; }
        public int? ThirdPartyTypeKey { get; set; }
        public string ThirdPartyTypeDescription { get; set; }
        
        public List<IRelationshipDefinition> Relationships { get; set; }

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
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "LegalEntityType",
                DataModelType = typeof(LookupDataModel),
                RelationshipType = RelationshipType.OneToOneLookup,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "LegalEntityTypeKey", RelatedKey = "LegalEntityTypeKey", Value = ""}
                    }
            });
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "ContactInformation",
                DataModelType = typeof(ThirdPartyContactInformationDataModel),
                RelationshipType = RelationshipType.OneToOne,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "Id", RelatedKey = "Id", Value = ""}
                    }
            });

            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "BankAccounts",
                DataModelType = typeof(IEnumerable<ThirdPartyBankAccountDataModel>),
                RelationshipType = RelationshipType.OneToManyWhere,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "Id", RelatedKey = "Id", Value = ""}
                    }
            });

            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "Invoices",
                DataModelType = typeof(IEnumerable<ThirdPartyInvoiceDataModel>),
                RelationshipType = RelationshipType.OneToManyWhere,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "ThirdPartyKey", RelatedKey = "ThirdPartyKey", Value = ""}
                    }
            });
            
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "PaymentBankAccount",
                DataModelType = typeof(ThirdPartyPaymentBankAccountDataModel),
                RelationshipType = RelationshipType.OneToOne,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "Id", RelatedKey = "ThirdPartyId", Value = ""}
                    }
            });

        }

    }
}