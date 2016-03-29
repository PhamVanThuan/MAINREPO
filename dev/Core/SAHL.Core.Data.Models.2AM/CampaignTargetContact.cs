using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CampaignTargetContactDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CampaignTargetContactDataModel(int campaignTargetKey, int legalEntityKey, DateTime changeDate, int adUserKey, int campaignTargetResponseKey)
        {
            this.CampaignTargetKey = campaignTargetKey;
            this.LegalEntityKey = legalEntityKey;
            this.ChangeDate = changeDate;
            this.AdUserKey = adUserKey;
            this.CampaignTargetResponseKey = campaignTargetResponseKey;
		
        }
		[JsonConstructor]
        public CampaignTargetContactDataModel(int campaignTargetContactKey, int campaignTargetKey, int legalEntityKey, DateTime changeDate, int adUserKey, int campaignTargetResponseKey)
        {
            this.CampaignTargetContactKey = campaignTargetContactKey;
            this.CampaignTargetKey = campaignTargetKey;
            this.LegalEntityKey = legalEntityKey;
            this.ChangeDate = changeDate;
            this.AdUserKey = adUserKey;
            this.CampaignTargetResponseKey = campaignTargetResponseKey;
		
        }		

        public int CampaignTargetContactKey { get; set; }

        public int CampaignTargetKey { get; set; }

        public int LegalEntityKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public int AdUserKey { get; set; }

        public int CampaignTargetResponseKey { get; set; }

        public void SetKey(int key)
        {
            this.CampaignTargetContactKey =  key;
        }
    }
}