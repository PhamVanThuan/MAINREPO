using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReasonDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ReasonDefinitionDataModel(int reasonTypeKey, bool allowComment, int reasonDescriptionKey, bool enforceComment, int generalStatusKey)
        {
            this.ReasonTypeKey = reasonTypeKey;
            this.AllowComment = allowComment;
            this.ReasonDescriptionKey = reasonDescriptionKey;
            this.EnforceComment = enforceComment;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public ReasonDefinitionDataModel(int reasonDefinitionKey, int reasonTypeKey, bool allowComment, int reasonDescriptionKey, bool enforceComment, int generalStatusKey)
        {
            this.ReasonDefinitionKey = reasonDefinitionKey;
            this.ReasonTypeKey = reasonTypeKey;
            this.AllowComment = allowComment;
            this.ReasonDescriptionKey = reasonDescriptionKey;
            this.EnforceComment = enforceComment;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int ReasonDefinitionKey { get; set; }

        public int ReasonTypeKey { get; set; }

        public bool AllowComment { get; set; }

        public int ReasonDescriptionKey { get; set; }

        public bool EnforceComment { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.ReasonDefinitionKey =  key;
        }
    }
}