using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StageTransitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StageTransitionDataModel(int genericKey, int aDUserKey, DateTime transitionDate, string comments, int stageDefinitionStageDefinitionGroupKey, DateTime? endTransitionDate)
        {
            this.GenericKey = genericKey;
            this.ADUserKey = aDUserKey;
            this.TransitionDate = transitionDate;
            this.Comments = comments;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.EndTransitionDate = endTransitionDate;
		
        }
		[JsonConstructor]
        public StageTransitionDataModel(int stageTransitionKey, int genericKey, int aDUserKey, DateTime transitionDate, string comments, int stageDefinitionStageDefinitionGroupKey, DateTime? endTransitionDate)
        {
            this.StageTransitionKey = stageTransitionKey;
            this.GenericKey = genericKey;
            this.ADUserKey = aDUserKey;
            this.TransitionDate = transitionDate;
            this.Comments = comments;
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.EndTransitionDate = endTransitionDate;
		
        }		

        public int StageTransitionKey { get; set; }

        public int GenericKey { get; set; }

        public int ADUserKey { get; set; }

        public DateTime TransitionDate { get; set; }

        public string Comments { get; set; }

        public int StageDefinitionStageDefinitionGroupKey { get; set; }

        public DateTime? EndTransitionDate { get; set; }

        public void SetKey(int key)
        {
            this.StageTransitionKey =  key;
        }
    }
}