using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FinancialServiceGroupDataModel :  IDataModel
    {
        public FinancialServiceGroupDataModel(int financialServiceGroupKey, string description)
        {
            this.FinancialServiceGroupKey = financialServiceGroupKey;
            this.Description = description;
		
        }		

        public int FinancialServiceGroupKey { get; set; }

        public string Description { get; set; }
    }
}