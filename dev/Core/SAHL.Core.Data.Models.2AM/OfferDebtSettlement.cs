using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDebtSettlementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferDebtSettlementDataModel(int? offerExpenseKey, double? settlementAmount, DateTime? settlementDate, int disbursementTypeKey, int? bankAccountKey, int? interestAppliedTypeKey, double? rateApplied, DateTime? interestStartDate, double? capitalAmount, double? guaranteeAmount, int? disbursementKey)
        {
            this.OfferExpenseKey = offerExpenseKey;
            this.SettlementAmount = settlementAmount;
            this.SettlementDate = settlementDate;
            this.DisbursementTypeKey = disbursementTypeKey;
            this.BankAccountKey = bankAccountKey;
            this.InterestAppliedTypeKey = interestAppliedTypeKey;
            this.RateApplied = rateApplied;
            this.InterestStartDate = interestStartDate;
            this.CapitalAmount = capitalAmount;
            this.GuaranteeAmount = guaranteeAmount;
            this.DisbursementKey = disbursementKey;
		
        }
		[JsonConstructor]
        public OfferDebtSettlementDataModel(int offerDebtSettlementKey, int? offerExpenseKey, double? settlementAmount, DateTime? settlementDate, int disbursementTypeKey, int? bankAccountKey, int? interestAppliedTypeKey, double? rateApplied, DateTime? interestStartDate, double? capitalAmount, double? guaranteeAmount, int? disbursementKey)
        {
            this.OfferDebtSettlementKey = offerDebtSettlementKey;
            this.OfferExpenseKey = offerExpenseKey;
            this.SettlementAmount = settlementAmount;
            this.SettlementDate = settlementDate;
            this.DisbursementTypeKey = disbursementTypeKey;
            this.BankAccountKey = bankAccountKey;
            this.InterestAppliedTypeKey = interestAppliedTypeKey;
            this.RateApplied = rateApplied;
            this.InterestStartDate = interestStartDate;
            this.CapitalAmount = capitalAmount;
            this.GuaranteeAmount = guaranteeAmount;
            this.DisbursementKey = disbursementKey;
		
        }		

        public int OfferDebtSettlementKey { get; set; }

        public int? OfferExpenseKey { get; set; }

        public double? SettlementAmount { get; set; }

        public DateTime? SettlementDate { get; set; }

        public int DisbursementTypeKey { get; set; }

        public int? BankAccountKey { get; set; }

        public int? InterestAppliedTypeKey { get; set; }

        public double? RateApplied { get; set; }

        public DateTime? InterestStartDate { get; set; }

        public double? CapitalAmount { get; set; }

        public double? GuaranteeAmount { get; set; }

        public int? DisbursementKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferDebtSettlementKey =  key;
        }
    }
}