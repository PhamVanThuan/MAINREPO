using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountSubsidyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountSubsidyDataModel(int accountKey, int subsidyKey)
        {
            this.AccountKey = accountKey;
            this.SubsidyKey = subsidyKey;
		
        }
		[JsonConstructor]
        public AccountSubsidyDataModel(int accountSubsidyKey, int accountKey, int subsidyKey)
        {
            this.AccountSubsidyKey = accountSubsidyKey;
            this.AccountKey = accountKey;
            this.SubsidyKey = subsidyKey;
		
        }		

        public int AccountSubsidyKey { get; set; }

        public int AccountKey { get; set; }

        public int SubsidyKey { get; set; }

        public void SetKey(int key)
        {
            this.AccountSubsidyKey =  key;
        }
    }
}