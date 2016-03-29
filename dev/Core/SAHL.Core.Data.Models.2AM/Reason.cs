using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReasonDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ReasonDataModel(int reasonDefinitionKey, int? genericKey, string comment, int? stageTransitionKey)
        {
            this.ReasonDefinitionKey = reasonDefinitionKey;
            this.GenericKey = genericKey;
            this.Comment = comment;
            this.StageTransitionKey = stageTransitionKey;
		
        }
		[JsonConstructor]
        public ReasonDataModel(int reasonKey, int reasonDefinitionKey, int? genericKey, string comment, int? stageTransitionKey)
        {
            this.ReasonKey = reasonKey;
            this.ReasonDefinitionKey = reasonDefinitionKey;
            this.GenericKey = genericKey;
            this.Comment = comment;
            this.StageTransitionKey = stageTransitionKey;
		
        }		

        public int ReasonKey { get; set; }

        public int ReasonDefinitionKey { get; set; }

        public int? GenericKey { get; set; }

        public string Comment { get; set; }

        public int? StageTransitionKey { get; set; }

        public void SetKey(int key)
        {
            this.ReasonKey =  key;
        }
    }
}