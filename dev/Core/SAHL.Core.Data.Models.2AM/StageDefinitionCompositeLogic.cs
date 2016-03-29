using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StageDefinitionCompositeLogicDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StageDefinitionCompositeLogicDataModel(int? stageDefinitionCompositeKey, int order, int operatorKey, int stageDefinitionStageDefinitionGroupKey)
        {
            this.StageDefinitionCompositeKey = stageDefinitionCompositeKey;
            this.Order = order;
            this.OperatorKey = operatorKey;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
		
        }
		[JsonConstructor]
        public StageDefinitionCompositeLogicDataModel(int stageDefinitionCompositeLogicKey, int? stageDefinitionCompositeKey, int order, int operatorKey, int stageDefinitionStageDefinitionGroupKey)
        {
            this.StageDefinitionCompositeLogicKey = stageDefinitionCompositeLogicKey;
            this.StageDefinitionCompositeKey = stageDefinitionCompositeKey;
            this.Order = order;
            this.OperatorKey = operatorKey;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
		
        }		

        public int StageDefinitionCompositeLogicKey { get; set; }

        public int? StageDefinitionCompositeKey { get; set; }

        public int Order { get; set; }

        public int OperatorKey { get; set; }

        public int StageDefinitionStageDefinitionGroupKey { get; set; }

        public void SetKey(int key)
        {
            this.StageDefinitionCompositeLogicKey =  key;
        }
    }
}