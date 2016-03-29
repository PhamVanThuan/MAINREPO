using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountTypeRecognitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountTypeRecognitionDataModel(int aCBBankCode, int aCBTypeNumber, long? rangeStart, long? rangeEnd, int? noOfDigits1, int? noOfDigits2, int? digitNo1, int? mustEqual1, int? digitNo2, int? mustEqual2, string dropDigits, int? startDropDigits, int? endDropDigits, string userID, DateTime dateChange)
        {
            this.ACBBankCode = aCBBankCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.RangeStart = rangeStart;
            this.RangeEnd = rangeEnd;
            this.NoOfDigits1 = noOfDigits1;
            this.NoOfDigits2 = noOfDigits2;
            this.DigitNo1 = digitNo1;
            this.MustEqual1 = mustEqual1;
            this.DigitNo2 = digitNo2;
            this.MustEqual2 = mustEqual2;
            this.DropDigits = dropDigits;
            this.StartDropDigits = startDropDigits;
            this.EndDropDigits = endDropDigits;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }
		[JsonConstructor]
        public AccountTypeRecognitionDataModel(int accountTypeRecognitionKey, int aCBBankCode, int aCBTypeNumber, long? rangeStart, long? rangeEnd, int? noOfDigits1, int? noOfDigits2, int? digitNo1, int? mustEqual1, int? digitNo2, int? mustEqual2, string dropDigits, int? startDropDigits, int? endDropDigits, string userID, DateTime dateChange)
        {
            this.AccountTypeRecognitionKey = accountTypeRecognitionKey;
            this.ACBBankCode = aCBBankCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.RangeStart = rangeStart;
            this.RangeEnd = rangeEnd;
            this.NoOfDigits1 = noOfDigits1;
            this.NoOfDigits2 = noOfDigits2;
            this.DigitNo1 = digitNo1;
            this.MustEqual1 = mustEqual1;
            this.DigitNo2 = digitNo2;
            this.MustEqual2 = mustEqual2;
            this.DropDigits = dropDigits;
            this.StartDropDigits = startDropDigits;
            this.EndDropDigits = endDropDigits;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }		

        public int AccountTypeRecognitionKey { get; set; }

        public int ACBBankCode { get; set; }

        public int ACBTypeNumber { get; set; }

        public long? RangeStart { get; set; }

        public long? RangeEnd { get; set; }

        public int? NoOfDigits1 { get; set; }

        public int? NoOfDigits2 { get; set; }

        public int? DigitNo1 { get; set; }

        public int? MustEqual1 { get; set; }

        public int? DigitNo2 { get; set; }

        public int? MustEqual2 { get; set; }

        public string DropDigits { get; set; }

        public int? StartDropDigits { get; set; }

        public int? EndDropDigits { get; set; }

        public string UserID { get; set; }

        public DateTime DateChange { get; set; }

        public void SetKey(int key)
        {
            this.AccountTypeRecognitionKey =  key;
        }
    }
}