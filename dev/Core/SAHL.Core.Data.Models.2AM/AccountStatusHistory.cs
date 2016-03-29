using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountStatusHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountStatusHistoryDataModel(int accountStatusKey, int accountKey, DateTime dateAssumed, string comment)
        {
            this.AccountStatusKey = accountStatusKey;
            this.AccountKey = accountKey;
            this.DateAssumed = dateAssumed;
            this.Comment = comment;
		
        }
		[JsonConstructor]
        public AccountStatusHistoryDataModel(int accountStatusHistoryKey, int accountStatusKey, int accountKey, DateTime dateAssumed, string comment)
        {
            this.AccountStatusHistoryKey = accountStatusHistoryKey;
            this.AccountStatusKey = accountStatusKey;
            this.AccountKey = accountKey;
            this.DateAssumed = dateAssumed;
            this.Comment = comment;
		
        }		

        public int AccountStatusHistoryKey { get; set; }

        public int AccountStatusKey { get; set; }

        public int AccountKey { get; set; }

        public DateTime DateAssumed { get; set; }

        public string Comment { get; set; }

        public void SetKey(int key)
        {
            this.AccountStatusHistoryKey =  key;
        }
    }
}