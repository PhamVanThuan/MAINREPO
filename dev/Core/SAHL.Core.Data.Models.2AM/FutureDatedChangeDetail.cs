using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FutureDatedChangeDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FutureDatedChangeDetailDataModel(int futureDatedChangeKey, int? referenceKey, string action, string tableName, string columnName, string value, string userID, DateTime? changeDate)
        {
            this.FutureDatedChangeKey = futureDatedChangeKey;
            this.ReferenceKey = referenceKey;
            this.Action = action;
            this.TableName = tableName;
            this.ColumnName = columnName;
            this.Value = value;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public FutureDatedChangeDetailDataModel(int futureDatedChangeDetailKey, int futureDatedChangeKey, int? referenceKey, string action, string tableName, string columnName, string value, string userID, DateTime? changeDate)
        {
            this.FutureDatedChangeDetailKey = futureDatedChangeDetailKey;
            this.FutureDatedChangeKey = futureDatedChangeKey;
            this.ReferenceKey = referenceKey;
            this.Action = action;
            this.TableName = tableName;
            this.ColumnName = columnName;
            this.Value = value;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public int FutureDatedChangeDetailKey { get; set; }

        public int FutureDatedChangeKey { get; set; }

        public int? ReferenceKey { get; set; }

        public string Action { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string Value { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.FutureDatedChangeDetailKey =  key;
        }
    }
}