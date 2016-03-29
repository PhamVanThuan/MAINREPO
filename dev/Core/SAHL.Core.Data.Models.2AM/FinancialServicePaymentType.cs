using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FinancialServicePaymentTypeDataModel :  IDataModel
    {
        public FinancialServicePaymentTypeDataModel(int financialServicePaymentTypeKey, string description)
        {
            this.FinancialServicePaymentTypeKey = financialServicePaymentTypeKey;
            this.Description = description;
		
        }		

        public int FinancialServicePaymentTypeKey { get; set; }

        public string Description { get; set; }
    }
}