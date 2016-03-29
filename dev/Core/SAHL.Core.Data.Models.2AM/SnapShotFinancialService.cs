using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SnapShotFinancialServiceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SnapShotFinancialServiceDataModel(int snapShotAccountKey, int accountKey, int financialServiceKey, int financialServiceTypeKey, int resetConfigurationKey, double? activeMarketRate, int? marginKey, double? installment, int? parentFinancialServiceKey)
        {
            this.SnapShotAccountKey = snapShotAccountKey;
            this.AccountKey = accountKey;
            this.FinancialServiceKey = financialServiceKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.ActiveMarketRate = activeMarketRate;
            this.MarginKey = marginKey;
            this.Installment = installment;
            this.ParentFinancialServiceKey = parentFinancialServiceKey;
		
        }
		[JsonConstructor]
        public SnapShotFinancialServiceDataModel(int snapShotFinancialServiceKey, int snapShotAccountKey, int accountKey, int financialServiceKey, int financialServiceTypeKey, int resetConfigurationKey, double? activeMarketRate, int? marginKey, double? installment, int? parentFinancialServiceKey)
        {
            this.SnapShotFinancialServiceKey = snapShotFinancialServiceKey;
            this.SnapShotAccountKey = snapShotAccountKey;
            this.AccountKey = accountKey;
            this.FinancialServiceKey = financialServiceKey;
            this.FinancialServiceTypeKey = financialServiceTypeKey;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.ActiveMarketRate = activeMarketRate;
            this.MarginKey = marginKey;
            this.Installment = installment;
            this.ParentFinancialServiceKey = parentFinancialServiceKey;
		
        }		

        public int SnapShotFinancialServiceKey { get; set; }

        public int SnapShotAccountKey { get; set; }

        public int AccountKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public int FinancialServiceTypeKey { get; set; }

        public int ResetConfigurationKey { get; set; }

        public double? ActiveMarketRate { get; set; }

        public int? MarginKey { get; set; }

        public double? Installment { get; set; }

        public int? ParentFinancialServiceKey { get; set; }

        public void SetKey(int key)
        {
            this.SnapShotFinancialServiceKey =  key;
        }
    }
}