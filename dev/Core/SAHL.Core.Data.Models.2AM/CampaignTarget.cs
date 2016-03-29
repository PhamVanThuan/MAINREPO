using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CampaignTargetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CampaignTargetDataModel(int campaignDefinitionKey, int genericKey, int aDUserKey, int genericKeyTypeKey)
        {
            this.CampaignDefinitionKey = campaignDefinitionKey;
            this.GenericKey = genericKey;
            this.ADUserKey = aDUserKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public CampaignTargetDataModel(int campaignTargetKey, int campaignDefinitionKey, int genericKey, int aDUserKey, int genericKeyTypeKey)
        {
            this.CampaignTargetKey = campaignTargetKey;
            this.CampaignDefinitionKey = campaignDefinitionKey;
            this.GenericKey = genericKey;
            this.ADUserKey = aDUserKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int CampaignTargetKey { get; set; }

        public int CampaignDefinitionKey { get; set; }

        public int GenericKey { get; set; }

        public int ADUserKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.CampaignTargetKey =  key;
        }
    }
}