using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BankRangeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BankRangeDataModel(int aCBBankCode, int rangeStart, int rangeEnd, string userID, DateTime dateChange)
        {
            this.ACBBankCode = aCBBankCode;
            this.RangeStart = rangeStart;
            this.RangeEnd = rangeEnd;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }
		[JsonConstructor]
        public BankRangeDataModel(int bankRangeKey, int aCBBankCode, int rangeStart, int rangeEnd, string userID, DateTime dateChange)
        {
            this.BankRangeKey = bankRangeKey;
            this.ACBBankCode = aCBBankCode;
            this.RangeStart = rangeStart;
            this.RangeEnd = rangeEnd;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }		

        public int BankRangeKey { get; set; }

        public int ACBBankCode { get; set; }

        public int RangeStart { get; set; }

        public int RangeEnd { get; set; }

        public string UserID { get; set; }

        public DateTime DateChange { get; set; }

        public void SetKey(int key)
        {
            this.BankRangeKey =  key;
        }
    }
}