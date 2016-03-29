using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProposalItemDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProposalItemDataModel(int proposalKey, DateTime startDate, DateTime endDate, int? marketRateKey, double interestRate, double amount, double additionalAmount, int aDUserKey, DateTime createDate, double? instalmentPercent, double? annualEscalation, int startPeriod, int endPeriod)
        {
            this.ProposalKey = proposalKey;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.MarketRateKey = marketRateKey;
            this.InterestRate = interestRate;
            this.Amount = amount;
            this.AdditionalAmount = additionalAmount;
            this.ADUserKey = aDUserKey;
            this.CreateDate = createDate;
            this.InstalmentPercent = instalmentPercent;
            this.AnnualEscalation = annualEscalation;
            this.StartPeriod = startPeriod;
            this.EndPeriod = endPeriod;
		
        }
		[JsonConstructor]
        public ProposalItemDataModel(int proposalItemKey, int proposalKey, DateTime startDate, DateTime endDate, int? marketRateKey, double interestRate, double amount, double additionalAmount, int aDUserKey, DateTime createDate, double? instalmentPercent, double? annualEscalation, int startPeriod, int endPeriod)
        {
            this.ProposalItemKey = proposalItemKey;
            this.ProposalKey = proposalKey;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.MarketRateKey = marketRateKey;
            this.InterestRate = interestRate;
            this.Amount = amount;
            this.AdditionalAmount = additionalAmount;
            this.ADUserKey = aDUserKey;
            this.CreateDate = createDate;
            this.InstalmentPercent = instalmentPercent;
            this.AnnualEscalation = annualEscalation;
            this.StartPeriod = startPeriod;
            this.EndPeriod = endPeriod;
		
        }		

        public int ProposalItemKey { get; set; }

        public int ProposalKey { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? MarketRateKey { get; set; }

        public double InterestRate { get; set; }

        public double Amount { get; set; }

        public double AdditionalAmount { get; set; }

        public int ADUserKey { get; set; }

        public DateTime CreateDate { get; set; }

        public double? InstalmentPercent { get; set; }

        public double? AnnualEscalation { get; set; }

        public int StartPeriod { get; set; }

        public int EndPeriod { get; set; }

        public void SetKey(int key)
        {
            this.ProposalItemKey =  key;
        }
    }
}