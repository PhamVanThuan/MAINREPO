using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferITCCreditScoreDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferITCCreditScoreDataModel(int offerCreditScoreKey, int iTCCreditScoreKey, int creditScoreDecisionKey, DateTime scoreDate, bool primaryApplicant)
        {
            this.OfferCreditScoreKey = offerCreditScoreKey;
            this.ITCCreditScoreKey = iTCCreditScoreKey;
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.ScoreDate = scoreDate;
            this.PrimaryApplicant = primaryApplicant;
		
        }
		[JsonConstructor]
        public OfferITCCreditScoreDataModel(int offerITCCreditScoreKey, int offerCreditScoreKey, int iTCCreditScoreKey, int creditScoreDecisionKey, DateTime scoreDate, bool primaryApplicant)
        {
            this.OfferITCCreditScoreKey = offerITCCreditScoreKey;
            this.OfferCreditScoreKey = offerCreditScoreKey;
            this.ITCCreditScoreKey = iTCCreditScoreKey;
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.ScoreDate = scoreDate;
            this.PrimaryApplicant = primaryApplicant;
		
        }		

        public int OfferITCCreditScoreKey { get; set; }

        public int OfferCreditScoreKey { get; set; }

        public int ITCCreditScoreKey { get; set; }

        public int CreditScoreDecisionKey { get; set; }

        public DateTime ScoreDate { get; set; }

        public bool PrimaryApplicant { get; set; }

        public void SetKey(int key)
        {
            this.OfferITCCreditScoreKey =  key;
        }
    }
}