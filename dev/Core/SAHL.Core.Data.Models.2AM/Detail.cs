using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DetailDataModel(int detailTypeKey, int accountKey, DateTime detailDate, double? amount, string description, int? linkID, string userID, DateTime? changeDate)
        {
            this.DetailTypeKey = detailTypeKey;
            this.AccountKey = accountKey;
            this.DetailDate = detailDate;
            this.Amount = amount;
            this.Description = description;
            this.LinkID = linkID;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public DetailDataModel(int detailKey, int detailTypeKey, int accountKey, DateTime detailDate, double? amount, string description, int? linkID, string userID, DateTime? changeDate)
        {
            this.DetailKey = detailKey;
            this.DetailTypeKey = detailTypeKey;
            this.AccountKey = accountKey;
            this.DetailDate = detailDate;
            this.Amount = amount;
            this.Description = description;
            this.LinkID = linkID;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public int DetailKey { get; set; }

        public int DetailTypeKey { get; set; }

        public int AccountKey { get; set; }

        public DateTime DetailDate { get; set; }

        public double? Amount { get; set; }

        public string Description { get; set; }

        public int? LinkID { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.DetailKey =  key;
        }
    }
}