using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SANTAMPolicyTrackingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SANTAMPolicyTrackingDataModel(int sANTAMPolicyTrackingKey, string policyNumber, string quoteNumber, int campaignTargetContactKey, int legalEntityKey, int accountKey, DateTime activeDate, DateTime cancelDate, double monthlyPremium, int collectionDay, int sANTAMPolicyStatusKey)
        {
            this.SANTAMPolicyTrackingKey = sANTAMPolicyTrackingKey;
            this.PolicyNumber = policyNumber;
            this.QuoteNumber = quoteNumber;
            this.CampaignTargetContactKey = campaignTargetContactKey;
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.ActiveDate = activeDate;
            this.CancelDate = cancelDate;
            this.MonthlyPremium = monthlyPremium;
            this.CollectionDay = collectionDay;
            this.SANTAMPolicyStatusKey = sANTAMPolicyStatusKey;
		
        }
		[JsonConstructor]
        public SANTAMPolicyTrackingDataModel(int sANTAMPolicyTrackingPrimaryKey, int sANTAMPolicyTrackingKey, string policyNumber, string quoteNumber, int campaignTargetContactKey, int legalEntityKey, int accountKey, DateTime activeDate, DateTime cancelDate, double monthlyPremium, int collectionDay, int sANTAMPolicyStatusKey)
        {
            this.SANTAMPolicyTrackingPrimaryKey = sANTAMPolicyTrackingPrimaryKey;
            this.SANTAMPolicyTrackingKey = sANTAMPolicyTrackingKey;
            this.PolicyNumber = policyNumber;
            this.QuoteNumber = quoteNumber;
            this.CampaignTargetContactKey = campaignTargetContactKey;
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.ActiveDate = activeDate;
            this.CancelDate = cancelDate;
            this.MonthlyPremium = monthlyPremium;
            this.CollectionDay = collectionDay;
            this.SANTAMPolicyStatusKey = sANTAMPolicyStatusKey;
		
        }		

        public int SANTAMPolicyTrackingPrimaryKey { get; set; }

        public int SANTAMPolicyTrackingKey { get; set; }

        public string PolicyNumber { get; set; }

        public string QuoteNumber { get; set; }

        public int CampaignTargetContactKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AccountKey { get; set; }

        public DateTime ActiveDate { get; set; }

        public DateTime CancelDate { get; set; }

        public double MonthlyPremium { get; set; }

        public int CollectionDay { get; set; }

        public int SANTAMPolicyStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.SANTAMPolicyTrackingPrimaryKey =  key;
        }
    }
}