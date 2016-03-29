using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SnapShotAccountDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SnapShotAccountDataModel(int accountKey, int remainingInstallments, int? productKey, int? valuationKey, DateTime? insertDate, int debtCounsellingKey, double? hOCPremium, double? lifePremium, double? monthlyServiceFee)
        {
            this.AccountKey = accountKey;
            this.RemainingInstallments = remainingInstallments;
            this.ProductKey = productKey;
            this.ValuationKey = valuationKey;
            this.InsertDate = insertDate;
            this.DebtCounsellingKey = debtCounsellingKey;
            this.HOCPremium = hOCPremium;
            this.LifePremium = lifePremium;
            this.MonthlyServiceFee = monthlyServiceFee;
		
        }
		[JsonConstructor]
        public SnapShotAccountDataModel(int snapShotAccountKey, int accountKey, int remainingInstallments, int? productKey, int? valuationKey, DateTime? insertDate, int debtCounsellingKey, double? hOCPremium, double? lifePremium, double? monthlyServiceFee)
        {
            this.SnapShotAccountKey = snapShotAccountKey;
            this.AccountKey = accountKey;
            this.RemainingInstallments = remainingInstallments;
            this.ProductKey = productKey;
            this.ValuationKey = valuationKey;
            this.InsertDate = insertDate;
            this.DebtCounsellingKey = debtCounsellingKey;
            this.HOCPremium = hOCPremium;
            this.LifePremium = lifePremium;
            this.MonthlyServiceFee = monthlyServiceFee;
		
        }		

        public int SnapShotAccountKey { get; set; }

        public int AccountKey { get; set; }

        public int RemainingInstallments { get; set; }

        public int? ProductKey { get; set; }

        public int? ValuationKey { get; set; }

        public DateTime? InsertDate { get; set; }

        public int DebtCounsellingKey { get; set; }

        public double? HOCPremium { get; set; }

        public double? LifePremium { get; set; }

        public double? MonthlyServiceFee { get; set; }

        public void SetKey(int key)
        {
            this.SnapShotAccountKey =  key;
        }
    }
}