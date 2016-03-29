using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MemoDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MemoDataModel(int genericKeyTypeKey, int genericKey, DateTime insertedDate, string memo, int aDUserKey, DateTime? changeDate, int generalStatusKey, DateTime? reminderDate, DateTime? expiryDate)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.InsertedDate = insertedDate;
            this.Memo = memo;
            this.ADUserKey = aDUserKey;
            this.ChangeDate = changeDate;
            this.GeneralStatusKey = generalStatusKey;
            this.ReminderDate = reminderDate;
            this.ExpiryDate = expiryDate;
		
        }
		[JsonConstructor]
        public MemoDataModel(int memoKey, int genericKeyTypeKey, int genericKey, DateTime insertedDate, string memo, int aDUserKey, DateTime? changeDate, int generalStatusKey, DateTime? reminderDate, DateTime? expiryDate)
        {
            this.MemoKey = memoKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.InsertedDate = insertedDate;
            this.Memo = memo;
            this.ADUserKey = aDUserKey;
            this.ChangeDate = changeDate;
            this.GeneralStatusKey = generalStatusKey;
            this.ReminderDate = reminderDate;
            this.ExpiryDate = expiryDate;
		
        }		

        public int MemoKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public DateTime InsertedDate { get; set; }

        public string Memo { get; set; }

        public int ADUserKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime? ReminderDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public void SetKey(int key)
        {
            this.MemoKey =  key;
        }
    }
}