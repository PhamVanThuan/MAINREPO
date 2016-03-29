using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OperatorGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OperatorGroupDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public OperatorGroupDataModel(int operatorGroupKey, string description)
        {
            this.OperatorGroupKey = operatorGroupKey;
            this.Description = description;
		
        }		

        public int OperatorGroupKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.OperatorGroupKey =  key;
        }
    }
}