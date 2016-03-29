using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InvoiceStatusDataModel :  IDataModel
    {
        public InvoiceStatusDataModel(int invoiceStatusKey, string description)
        {
            this.InvoiceStatusKey = invoiceStatusKey;
            this.Description = description;
		
        }		

        public int InvoiceStatusKey { get; set; }

        public string Description { get; set; }
    }
}