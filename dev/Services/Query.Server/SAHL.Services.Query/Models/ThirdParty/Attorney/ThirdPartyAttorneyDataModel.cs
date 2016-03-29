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
    public class ThirdPartyAttorneyDataModel : IQueryDataModel
    {

        public ThirdPartyAttorneyDataModel()
        {
            SetupRelationships();
        }

        public Guid Id { get; set; }
        public int? ThirdPartyKey { get; set; }
        public int? LegalEntityKey { get; set; }
        public string LegalName { get; set; }
        public string RegistrationNumber { get; set; }

        public bool? IsLitigationAttorney { get; set; }
        public bool? IsRegistrationAttorney { get; set; }
        public bool? IsPanelAttorney { get; set; }
        public string AttorneyContact { get; set; }
        public int? DeedsOfficeKey { get; set; }
        public string DeedsOfficeDescription { get; set; }
        public decimal? Mandate { get; set; }
        public bool? WorkflowEnabled { get; set; }

        public int? GeneralStatusKey { get; set; }
        public string GeneralStatusDescription { get; set; }
        public int? LegalEntityTypeKey { get; set; }
        public string LegaLEntityStatusDescription { get; set; }

        public string WorkPhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string EmailAddress { get; set; }
        
        public List<IRelationshipDefinition> Relationships { get; set; }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
         
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