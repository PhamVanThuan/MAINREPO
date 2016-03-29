using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Query.Models.Finance
{
    public class ThirdPartyInvoiceLineItemsDataModel : IQueryDataModel
    {

        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
        public int? ThirdPartyInvoiceKey { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public string InvoiceStatus { get; set; }
        public string LineItemType { get; set; }
        public string LineItemDesc { get; set; }
        public int? InvoiceLineItemDescriptionKey { get; set; }
        public int? InvoiceLineItemCategoryKey { get; set; }
        public decimal? LineItemAmount { get; set; }
        public Boolean? IsVatable { get; set; }
        public Boolean? CapitaliseInvoice { get; set; }
        public decimal? LineItemVatAmount { get; set; }
        public decimal? LineItemTotalAmtInclVAT { get; set; }

        public ThirdPartyInvoiceLineItemsDataModel()
        {
            SetupRelationships();
        }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }
    }
}
