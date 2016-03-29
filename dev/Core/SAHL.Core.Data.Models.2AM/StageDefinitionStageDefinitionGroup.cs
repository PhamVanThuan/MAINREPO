using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StageDefinitionStageDefinitionGroupDataModel :  IDataModel
    {
        public StageDefinitionStageDefinitionGroupDataModel(int stageDefinitionStageDefinitionGroupKey, int stageDefinitionGroupKey, int stageDefinitionKey)
        {
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.StageDefinitionGroupKey = stageDefinitionGroupKey;
            this.StageDefinitionKey = stageDefinitionKey;
		
        }		

        public int StageDefinitionStageDefinitionGroupKey { get; set; }

        public int StageDefinitionGroupKey { get; set; }

        public int StageDefinitionKey { get; set; }
    }
}