using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FinancialServiceTypeDataModel :  IDataModel
    {
        public FinancialServiceTypeDataModel(int financialServiceTypeKey, string description, int? resetConfigurationKey, int? balanceTypeKey, int? financialServiceGroupKey)
        {
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.Description = description;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.BalanceTypeKey = balanceTypeKey;
            this.FinancialServiceGroupKey = financialServiceGroupKey;
		
        }		

        public int FinancialServiceTypeKey { get; set; }

        public string Description { get; set; }

        public int? ResetConfigurationKey { get; set; }

        public int? BalanceTypeKey { get; set; }

        public int? FinancialServiceGroupKey { get; set; }
    }
}