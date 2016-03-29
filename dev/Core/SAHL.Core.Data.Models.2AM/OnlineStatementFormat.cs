using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OnlineStatementFormatDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OnlineStatementFormatDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public OnlineStatementFormatDataModel(int onlineStatementFormatKey, string description)
        {
            this.OnlineStatementFormatKey = onlineStatementFormatKey;
            this.Description = description;
		
        }		

        public int OnlineStatementFormatKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.OnlineStatementFormatKey =  key;
        }
    }
}