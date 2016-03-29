using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TransactionTypeUIDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TransactionTypeUIDataModel(int transactionTypeKey, int screenBatch, string hTMLColour, bool memo)
        {
            this.TransactionTypeKey = transactionTypeKey;
            this.ScreenBatch = screenBatch;
            this.HTMLColour = hTMLColour;
            this.Memo = memo;
		
        }
		[JsonConstructor]
        public TransactionTypeUIDataModel(int transactionTypeUIKey, int transactionTypeKey, int screenBatch, string hTMLColour, bool memo)
        {
            this.TransactionTypeUIKey = transactionTypeUIKey;
            this.TransactionTypeKey = transactionTypeKey;
            this.ScreenBatch = screenBatch;
            this.HTMLColour = hTMLColour;
            this.Memo = memo;
		
        }		

        public int TransactionTypeUIKey { get; set; }

        public int TransactionTypeKey { get; set; }

        public int ScreenBatch { get; set; }

        public string HTMLColour { get; set; }

        public bool Memo { get; set; }

        public void SetKey(int key)
        {
            this.TransactionTypeUIKey =  key;
        }
    }
}