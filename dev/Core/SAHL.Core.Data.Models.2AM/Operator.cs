using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OperatorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OperatorDataModel(string description, int operatorGroupKey)
        {
            this.Description = description;
            this.OperatorGroupKey = operatorGroupKey;
		
        }
		[JsonConstructor]
        public OperatorDataModel(int operatorKey, string description, int operatorGroupKey)
        {
            this.OperatorKey = operatorKey;
            this.Description = description;
            this.OperatorGroupKey = operatorGroupKey;
		
        }		

        public int OperatorKey { get; set; }

        public string Description { get; set; }

        public int OperatorGroupKey { get; set; }

        public void SetKey(int key)
        {
            this.OperatorKey =  key;
        }
    }
}