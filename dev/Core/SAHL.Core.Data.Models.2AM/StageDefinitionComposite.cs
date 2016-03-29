using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StageDefinitionCompositeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StageDefinitionCompositeDataModel(int stageDefinitionStageDefinitionGroupCompositeKey, int stageDefinitionStageDefinitionGroupKey, bool useThisDate, int sequence, bool useThisReason)
        {
            this.StageDefinitionStageDefinitionGroupCompositeKey = stageDefinitionStageDefinitionGroupCompositeKey;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.UseThisDate = useThisDate;
            this.Sequence = sequence;
            this.UseThisReason = useThisReason;
		
        }
		[JsonConstructor]
        public StageDefinitionCompositeDataModel(int stageDefinitionCompositeKey, int stageDefinitionStageDefinitionGroupCompositeKey, int stageDefinitionStageDefinitionGroupKey, bool useThisDate, int sequence, bool useThisReason)
        {
            this.StageDefinitionCompositeKey = stageDefinitionCompositeKey;
            this.StageDefinitionStageDefinitionGroupCompositeKey = stageDefinitionStageDefinitionGroupCompositeKey;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.UseThisDate = useThisDate;
            this.Sequence = sequence;
            this.UseThisReason = useThisReason;
		
        }		

        public int StageDefinitionCompositeKey { get; set; }

        public int StageDefinitionStageDefinitionGroupCompositeKey { get; set; }

        public int StageDefinitionStageDefinitionGroupKey { get; set; }

        public bool UseThisDate { get; set; }

        public int Sequence { get; set; }

        public bool UseThisReason { get; set; }

        public void SetKey(int key)
        {
            this.StageDefinitionCompositeKey =  key;
        }
    }
}