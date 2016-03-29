using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountExternalLifeDataModel :  IDataModel
    {
        public AccountExternalLifeDataModel(int accountKey, int externalLifePolicyKey)
        {
            this.AccountKey = accountKey;
            this.ExternalLifePolicyKey = externalLifePolicyKey;
		
        }		

        public int AccountKey { get; set; }

        public int ExternalLifePolicyKey { get; set; }
    }
}