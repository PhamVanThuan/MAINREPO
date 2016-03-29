using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetOpenNewBusinessApplicationQueryResult
    {
        public GetOpenNewBusinessApplicationQueryResult(int offerKey, int offerTypeKey, int offerStatusKey, DateTime? offerStartDate, DateTime? offerEndDate, int? accountKey,
                                                        string reference, int? offerCampaignKey, int? offerSourceKey, int reservedAccountKey, int originationSourceKey,
                                                        int? estimateNumberApplicants)
        {
            this.OfferKey = offerKey;
            this.OfferTypeKey = offerTypeKey;
            this.OfferStatusKey = offerStatusKey;
            this.OfferStartDate = offerStartDate;
            this.OfferEndDate = offerEndDate;
            this.AccountKey = accountKey;
            this.Reference = reference;
            this.OfferCampaignKey = offerCampaignKey;
            this.OfferSourceKey = offerSourceKey;
            this.ReservedAccountKey = reservedAccountKey;
            this.OriginationSourceKey = originationSourceKey;
            this.EstimateNumberApplicants = estimateNumberApplicants;
        }

        public int OfferKey { get; set; }

        public int OfferTypeKey { get; set; }

        public int OfferStatusKey { get; set; }

        public DateTime? OfferStartDate { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public int? AccountKey { get; set; }

        public string Reference { get; set; }

        public int? OfferCampaignKey { get; set; }

        public int? OfferSourceKey { get; set; }

        public int ReservedAccountKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public int? EstimateNumberApplicants { get; set; }
    }
}