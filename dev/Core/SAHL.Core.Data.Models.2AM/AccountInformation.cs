using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountInformationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountInformationDataModel(int? accountInformationTypeKey, int? accountKey, DateTime? entryDate, double? amount, string information)
        {
            this.AccountInformationTypeKey = accountInformationTypeKey;
            this.AccountKey = accountKey;
            this.EntryDate = entryDate;
            this.Amount = amount;
            this.Information = information;
		
        }
		[JsonConstructor]
        public AccountInformationDataModel(int accountInformationKey, int? accountInformationTypeKey, int? accountKey, DateTime? entryDate, double? amount, string information)
        {
            this.AccountInformationKey = accountInformationKey;
            this.AccountInformationTypeKey = accountInformationTypeKey;
            this.AccountKey = accountKey;
            this.EntryDate = entryDate;
            this.Amount = amount;
            this.Information = information;
		
        }		

        public int AccountInformationKey { get; set; }

        public int? AccountInformationTypeKey { get; set; }

        public int? AccountKey { get; set; }

        public DateTime? EntryDate { get; set; }

        public double? Amount { get; set; }

        public string Information { get; set; }

        public void SetKey(int key)
        {
            this.AccountInformationKey =  key;
        }
    }
}