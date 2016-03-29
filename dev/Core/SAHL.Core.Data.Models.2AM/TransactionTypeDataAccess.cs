using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TransactionTypeDataAccessDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TransactionTypeDataAccessDataModel(string aDCredentials, int transactionTypeKey)
        {
            this.ADCredentials = aDCredentials;
            this.TransactionTypeKey = transactionTypeKey;
		
        }
		[JsonConstructor]
        public TransactionTypeDataAccessDataModel(int transactionTypeDataAccessKey, string aDCredentials, int transactionTypeKey)
        {
            this.TransactionTypeDataAccessKey = transactionTypeDataAccessKey;
            this.ADCredentials = aDCredentials;
            this.TransactionTypeKey = transactionTypeKey;
		
        }		

        public int TransactionTypeDataAccessKey { get; set; }

        public string ADCredentials { get; set; }

        public int TransactionTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.TransactionTypeDataAccessKey =  key;
        }
    }
}