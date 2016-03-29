using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SANTAMPolicyTracking_TEMPDataModel :  IDataModel
    {
        public SANTAMPolicyTracking_TEMPDataModel(int? sANTAMPolicyTrackingKey, string quoteNumber, string policyNumber, string iDNumber, string activeDate, string cancelDate, string monthlyPremium, string collectionDay, string sANTAMPolicyStatus, int? campaignTargetContactKey)
        {
            this.SANTAMPolicyTrackingKey = sANTAMPolicyTrackingKey;
            this.QuoteNumber = quoteNumber;
            this.PolicyNumber = policyNumber;
            this.IDNumber = iDNumber;
            this.ActiveDate = activeDate;
            this.CancelDate = cancelDate;
            this.MonthlyPremium = monthlyPremium;
            this.CollectionDay = collectionDay;
            this.SANTAMPolicyStatus = sANTAMPolicyStatus;
            this.CampaignTargetContactKey = campaignTargetContactKey;
		
        }		

        public int? SANTAMPolicyTrackingKey { get; set; }

        public string QuoteNumber { get; set; }

        public string PolicyNumber { get; set; }

        public string IDNumber { get; set; }

        public string ActiveDate { get; set; }

        public string CancelDate { get; set; }

        public string MonthlyPremium { get; set; }

        public string CollectionDay { get; set; }

        public string SANTAMPolicyStatus { get; set; }

        public int? CampaignTargetContactKey { get; set; }
    }
}