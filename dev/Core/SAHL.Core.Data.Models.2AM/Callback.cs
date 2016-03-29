using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CallbackDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CallbackDataModel(int genericKeyTypeKey, int genericKey, int reasonKey, DateTime entryDate, string entryUser, DateTime callbackDate, string callbackUser, DateTime? completedDate, string completedUser)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.ReasonKey = reasonKey;
            this.EntryDate = entryDate;
            this.EntryUser = entryUser;
            this.CallbackDate = callbackDate;
            this.CallbackUser = callbackUser;
            this.CompletedDate = completedDate;
            this.CompletedUser = completedUser;
		
        }
		[JsonConstructor]
        public CallbackDataModel(int callbackKey, int genericKeyTypeKey, int genericKey, int reasonKey, DateTime entryDate, string entryUser, DateTime callbackDate, string callbackUser, DateTime? completedDate, string completedUser)
        {
            this.CallbackKey = callbackKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.ReasonKey = reasonKey;
            this.EntryDate = entryDate;
            this.EntryUser = entryUser;
            this.CallbackDate = callbackDate;
            this.CallbackUser = callbackUser;
            this.CompletedDate = completedDate;
            this.CompletedUser = completedUser;
		
        }		

        public int CallbackKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public int ReasonKey { get; set; }

        public DateTime EntryDate { get; set; }

        public string EntryUser { get; set; }

        public DateTime CallbackDate { get; set; }

        public string CallbackUser { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string CompletedUser { get; set; }

        public void SetKey(int key)
        {
            this.CallbackKey =  key;
        }
    }
}