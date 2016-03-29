using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StageTransitionCompositeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StageTransitionCompositeDataModel(int stageTransitionKey, int genericKey, int aDUserKey, DateTime transitionDate, string comments, int stageDefinitionStageDefinitionGroupKey, int? stageTransitionReasonKey)
        {
            this.StageTransitionKey = stageTransitionKey;
            this.GenericKey = genericKey;
            this.ADUserKey = aDUserKey;
            this.TransitionDate = transitionDate;
            this.Comments = comments;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.StageTransitionReasonKey = stageTransitionReasonKey;
		
        }
		[JsonConstructor]
        public StageTransitionCompositeDataModel(int stageTransitionCompositeKey, int stageTransitionKey, int genericKey, int aDUserKey, DateTime transitionDate, string comments, int stageDefinitionStageDefinitionGroupKey, int? stageTransitionReasonKey)
        {
            this.StageTransitionCompositeKey = stageTransitionCompositeKey;
            this.StageTransitionKey = stageTransitionKey;
            this.GenericKey = genericKey;
            this.ADUserKey = aDUserKey;
            this.TransitionDate = transitionDate;
            this.Comments = comments;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.StageTransitionReasonKey = stageTransitionReasonKey;
		
        }		

        public int StageTransitionCompositeKey { get; set; }

        public int StageTransitionKey { get; set; }

        public int GenericKey { get; set; }

        public int ADUserKey { get; set; }

        public DateTime TransitionDate { get; set; }

        public string Comments { get; set; }

        public int StageDefinitionStageDefinitionGroupKey { get; set; }

        public int? StageTransitionReasonKey { get; set; }

        public void SetKey(int key)
        {
            this.StageTransitionCompositeKey =  key;
        }
    }
}