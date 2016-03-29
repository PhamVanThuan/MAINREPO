using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountPropertyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountPropertyDataModel(int? accountKey, int? propertyKey)
        {
            this.AccountKey = accountKey;
            this.PropertyKey = propertyKey;
		
        }
		[JsonConstructor]
        public AccountPropertyDataModel(int accountPropertyKey, int? accountKey, int? propertyKey)
        {
            this.AccountPropertyKey = accountPropertyKey;
            this.AccountKey = accountKey;
            this.PropertyKey = propertyKey;
		
        }		

        public int AccountPropertyKey { get; set; }

        public int? AccountKey { get; set; }

        public int? PropertyKey { get; set; }

        public void SetKey(int key)
        {
            this.AccountPropertyKey =  key;
        }
    }
}