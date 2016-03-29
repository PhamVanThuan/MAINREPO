using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HistoryNotMigrated_OriginationCleanUpDataModel :  IDataModel
    {
        public HistoryNotMigrated_OriginationCleanUpDataModel(int historyKey, int accountKey, int aDUserKey, string userName, DateTime date, string action, string comments)
        {
            this.HistoryKey = historyKey;
            this.AccountKey = accountKey;
            this.ADUserKey = aDUserKey;
            this.UserName = userName;
            this.date = date;
            this.Action = action;
            this.Comments = comments;
		
        }		

        public int HistoryKey { get; set; }

        public int AccountKey { get; set; }

        public int ADUserKey { get; set; }

        public string UserName { get; set; }

        public DateTime date { get; set; }

        public string Action { get; set; }

        public string Comments { get; set; }
    }
}