using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountSequenceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountSequenceDataModel(bool? isUsed)
        {
            this.IsUsed = isUsed;
		
        }
		[JsonConstructor]
        public AccountSequenceDataModel(int accountKey, bool? isUsed)
        {
            this.AccountKey = accountKey;
            this.IsUsed = isUsed;
		
        }		

        public int AccountKey { get; set; }

        public bool? IsUsed { get; set; }

        public void SetKey(int key)
        {
            this.AccountKey =  key;
        }
    }
}