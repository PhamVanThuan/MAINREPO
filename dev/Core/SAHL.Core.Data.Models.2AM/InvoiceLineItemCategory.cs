using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InvoiceLineItemCategoryDataModel :  IDataModel
    {
        public InvoiceLineItemCategoryDataModel(int invoiceLineItemCategoryKey, string invoiceLineItemCategory)
        {
            this.InvoiceLineItemCategoryKey = invoiceLineItemCategoryKey;
            this.InvoiceLineItemCategory = invoiceLineItemCategory;
		
        }		

        public int InvoiceLineItemCategoryKey { get; set; }

        public string InvoiceLineItemCategory { get; set; }
    }
}