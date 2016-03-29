using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionSetUIStatementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionSetUIStatementDataModel(int conditionSetKey, string uIStatementName)
        {
            this.ConditionSetKey = conditionSetKey;
            this.UIStatementName = uIStatementName;
		
        }
		[JsonConstructor]
        public ConditionSetUIStatementDataModel(int conditionSetUIStatementKey, int conditionSetKey, string uIStatementName)
        {
            this.ConditionSetUIStatementKey = conditionSetUIStatementKey;
            this.ConditionSetKey = conditionSetKey;
            this.UIStatementName = uIStatementName;
		
        }		

        public int ConditionSetUIStatementKey { get; set; }

        public int ConditionSetKey { get; set; }

        public string UIStatementName { get; set; }

        public void SetKey(int key)
        {
            this.ConditionSetUIStatementKey =  key;
        }
    }
}