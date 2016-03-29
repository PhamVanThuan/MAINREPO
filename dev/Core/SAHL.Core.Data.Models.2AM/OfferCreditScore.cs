using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferCreditScoreDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferCreditScoreDataModel(int offerKey, int offerAggregateDecisionKey, DateTime scoreDate, int? callingContextKey)
        {
            this.OfferKey = offerKey;
            this.OfferAggregateDecisionKey = offerAggregateDecisionKey;
            this.ScoreDate = scoreDate;
            this.CallingContextKey = callingContextKey;
		
        }
		[JsonConstructor]
        public OfferCreditScoreDataModel(int offerCreditScoreKey, int offerKey, int offerAggregateDecisionKey, DateTime scoreDate, int? callingContextKey)
        {
            this.OfferCreditScoreKey = offerCreditScoreKey;
            this.OfferKey = offerKey;
            this.OfferAggregateDecisionKey = offerAggregateDecisionKey;
            this.ScoreDate = scoreDate;
            this.CallingContextKey = callingContextKey;
		
        }		

        public int OfferCreditScoreKey { get; set; }

        public int OfferKey { get; set; }

        public int OfferAggregateDecisionKey { get; set; }

        public DateTime ScoreDate { get; set; }

        public int? CallingContextKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferCreditScoreKey =  key;
        }
    }
}