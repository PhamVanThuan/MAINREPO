using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CampaignTargetResponseDataModel :  IDataModel
    {
        public CampaignTargetResponseDataModel(int campaignTargetResponseKey, string description)
        {
            this.CampaignTargetResponseKey = campaignTargetResponseKey;
            this.Description = description;
		
        }		

        public int CampaignTargetResponseKey { get; set; }

        public string Description { get; set; }
    }
}