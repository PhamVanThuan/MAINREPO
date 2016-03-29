using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ITCDecisionReasonDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ITCDecisionReasonDataModel(int iTCCreditScoreKey, int? offerCreditScoreKey, int creditScoreDecisionKey, int? reasonKey)
        {
            this.ITCCreditScoreKey = iTCCreditScoreKey;
            this.OfferCreditScoreKey = offerCreditScoreKey;
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.ReasonKey = reasonKey;
		
        }
		[JsonConstructor]
        public ITCDecisionReasonDataModel(int iTCDecisionReasonKey, int iTCCreditScoreKey, int? offerCreditScoreKey, int creditScoreDecisionKey, int? reasonKey)
        {
            this.ITCDecisionReasonKey = iTCDecisionReasonKey;
            this.ITCCreditScoreKey = iTCCreditScoreKey;
            this.OfferCreditScoreKey = offerCreditScoreKey;
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.ReasonKey = reasonKey;
		
        }		

        public int ITCDecisionReasonKey { get; set; }

        public int ITCCreditScoreKey { get; set; }

        public int? OfferCreditScoreKey { get; set; }

        public int CreditScoreDecisionKey { get; set; }

        public int? ReasonKey { get; set; }

        public void SetKey(int key)
        {
            this.ITCDecisionReasonKey =  key;
        }
    }
}