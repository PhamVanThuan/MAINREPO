using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferAggregateDecisionDataModel :  IDataModel
    {
        public OfferAggregateDecisionDataModel(int offerAggregateDecisionKey, int primaryDecision, int? secondaryDecision, int aggregateDecision)
        {
            this.OfferAggregateDecisionKey = offerAggregateDecisionKey;
            this.PrimaryDecision = primaryDecision;
            this.SecondaryDecision = secondaryDecision;
            this.AggregateDecision = aggregateDecision;
		
        }		

        public int OfferAggregateDecisionKey { get; set; }

        public int PrimaryDecision { get; set; }

        public int? SecondaryDecision { get; set; }

        public int AggregateDecision { get; set; }
    }
}