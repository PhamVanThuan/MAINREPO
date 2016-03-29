using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class StageActivityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StageActivityDataModel(int activityID, int? stageDefinitionKey, int? stageDefinitionStageDefinitionGroupKey)
        {
            this.ActivityID = activityID;
            this.StageDefinitionKey = stageDefinitionKey;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
		
        }
		[JsonConstructor]
        public StageActivityDataModel(int iD, int activityID, int? stageDefinitionKey, int? stageDefinitionStageDefinitionGroupKey)
        {
            this.ID = iD;
            this.ActivityID = activityID;
            this.StageDefinitionKey = stageDefinitionKey;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
		
        }		

        public int ID { get; set; }

        public int ActivityID { get; set; }

        public int? StageDefinitionKey { get; set; }

        public int? StageDefinitionStageDefinitionGroupKey { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}