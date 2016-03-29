using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentSetConfigDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentSetConfigDataModel(int documentSetKey, int documentTypeKey, int ruleItemKey)
        {
            this.DocumentSetKey = documentSetKey;
            this.DocumentTypeKey = documentTypeKey;
            this.RuleItemKey = ruleItemKey;
		
        }
		[JsonConstructor]
        public DocumentSetConfigDataModel(int documentSetConfigKey, int documentSetKey, int documentTypeKey, int ruleItemKey)
        {
            this.DocumentSetConfigKey = documentSetConfigKey;
            this.DocumentSetKey = documentSetKey;
            this.DocumentTypeKey = documentTypeKey;
            this.RuleItemKey = ruleItemKey;
		
        }		

        public int DocumentSetConfigKey { get; set; }

        public int DocumentSetKey { get; set; }

        public int DocumentTypeKey { get; set; }

        public int RuleItemKey { get; set; }

        public void SetKey(int key)
        {
            this.DocumentSetConfigKey =  key;
        }
    }
}