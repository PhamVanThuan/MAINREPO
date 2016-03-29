using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ForeclosureAuctionInformationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ForeclosureAuctionInformationDataModel(int? auctionCompanyLegalEntityKey, int? foreclosureAuctionOutcomeKey, int? memoKey, int aDUserKey, int foreclosureAuctionKey, DateTime? auctionDateTime, double? forcedSaleValue, double? reservePrice, double? salePrice, double? lossApproved, double? balanceAtOutcome, DateTime? changeDate, string workflowUser)
        {
            this.AuctionCompanyLegalEntityKey = auctionCompanyLegalEntityKey;
            this.ForeclosureAuctionOutcomeKey = foreclosureAuctionOutcomeKey;
            this.MemoKey = memoKey;
            this.ADUserKey = aDUserKey;
            this.ForeclosureAuctionKey = foreclosureAuctionKey;
            this.AuctionDateTime = auctionDateTime;
            this.ForcedSaleValue = forcedSaleValue;
            this.ReservePrice = reservePrice;
            this.SalePrice = salePrice;
            this.LossApproved = lossApproved;
            this.BalanceAtOutcome = balanceAtOutcome;
            this.ChangeDate = changeDate;
            this.WorkflowUser = workflowUser;
		
        }
		[JsonConstructor]
        public ForeclosureAuctionInformationDataModel(int foreclosureAuctionInformationKey, int? auctionCompanyLegalEntityKey, int? foreclosureAuctionOutcomeKey, int? memoKey, int aDUserKey, int foreclosureAuctionKey, DateTime? auctionDateTime, double? forcedSaleValue, double? reservePrice, double? salePrice, double? lossApproved, double? balanceAtOutcome, DateTime? changeDate, string workflowUser)
        {
            this.ForeclosureAuctionInformationKey = foreclosureAuctionInformationKey;
            this.AuctionCompanyLegalEntityKey = auctionCompanyLegalEntityKey;
            this.ForeclosureAuctionOutcomeKey = foreclosureAuctionOutcomeKey;
            this.MemoKey = memoKey;
            this.ADUserKey = aDUserKey;
            this.ForeclosureAuctionKey = foreclosureAuctionKey;
            this.AuctionDateTime = auctionDateTime;
            this.ForcedSaleValue = forcedSaleValue;
            this.ReservePrice = reservePrice;
            this.SalePrice = salePrice;
            this.LossApproved = lossApproved;
            this.BalanceAtOutcome = balanceAtOutcome;
            this.ChangeDate = changeDate;
            this.WorkflowUser = workflowUser;
		
        }		

        public int ForeclosureAuctionInformationKey { get; set; }

        public int? AuctionCompanyLegalEntityKey { get; set; }

        public int? ForeclosureAuctionOutcomeKey { get; set; }

        public int? MemoKey { get; set; }

        public int ADUserKey { get; set; }

        public int ForeclosureAuctionKey { get; set; }

        public DateTime? AuctionDateTime { get; set; }

        public double? ForcedSaleValue { get; set; }

        public double? ReservePrice { get; set; }

        public double? SalePrice { get; set; }

        public double? LossApproved { get; set; }

        public double? BalanceAtOutcome { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string WorkflowUser { get; set; }

        public void SetKey(int key)
        {
            this.ForeclosureAuctionInformationKey =  key;
        }
    }
}