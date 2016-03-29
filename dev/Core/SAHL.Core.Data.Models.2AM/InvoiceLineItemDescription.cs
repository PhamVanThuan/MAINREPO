using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InvoiceLineItemDescriptionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public InvoiceLineItemDescriptionDataModel(int invoiceLineItemCategoryKey, string invoiceLineItemDescription)
        {
            this.InvoiceLineItemCategoryKey = invoiceLineItemCategoryKey;
            this.InvoiceLineItemDescription = invoiceLineItemDescription;
		
        }
		[JsonConstructor]
        public InvoiceLineItemDescriptionDataModel(int invoiceLineItemDescriptionKey, int invoiceLineItemCategoryKey, string invoiceLineItemDescription)
        {
            this.InvoiceLineItemDescriptionKey = invoiceLineItemDescriptionKey;
            this.InvoiceLineItemCategoryKey = invoiceLineItemCategoryKey;
            this.InvoiceLineItemDescription = invoiceLineItemDescription;
		
        }		

        public int InvoiceLineItemDescriptionKey { get; set; }

        public int InvoiceLineItemCategoryKey { get; set; }

        public string InvoiceLineItemDescription { get; set; }

        public void SetKey(int key)
        {
            this.InvoiceLineItemDescriptionKey =  key;
        }
    }
}