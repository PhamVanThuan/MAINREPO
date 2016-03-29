using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models.Core;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Query.Models.Finance
{
    public class ThirdPartyInvoiceDataModel : IQueryDataModel
    {
        public ThirdPartyInvoiceDataModel()
        {
            SetupRelationships();
        }

        public List<IRelationshipDefinition> Relationships
        { get; set; }

        public int Id { get; set; }
        public int? LegalEntityKey { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public int? ThirdPartyKey { get; set; }
        public int? AccountKey { get; set; }
        public int? InvoiceStatusKey { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceNumber { get; set; }
        public string ClientName { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string SahlReference { get; set; }
        public string ReceivedFromEmailAddress { get; set; }
        public bool? CapitaliseInvoice { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? AmountExcludingVAT { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? TotalAmountIncludingVAT { get; set; }
        public string PaymentReference { get; set; }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();

            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "Documents",
                DataModelType = typeof(IEnumerable<ThirdPartyInvoiceDocumentDataModel>),
                RelationshipType = RelationshipType.OneToMany,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "Id", RelatedKey = "InvoiceId", Value = ""}
                    }
            });

            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "lineItemsList",
                DataModelType = typeof(IEnumerable<ThirdPartyInvoiceLineItemsDataModel>),
                RelationshipType = RelationshipType.OneToMany,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "Id", RelatedKey = "InvoiceId", Value = ""}
                    }
            });

            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "account",
                DataModelType = typeof(ThirdPartyInvoiceAccountDataModel),
                RelationshipType = RelationshipType.OneToOne,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "Id", RelatedKey = "Id", Value = ""}
                    }
            });

        }


    }
}
