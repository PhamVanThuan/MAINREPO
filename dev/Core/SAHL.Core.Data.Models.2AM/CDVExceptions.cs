using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CDVExceptionsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CDVExceptionsDataModel(int aCBBankCode, int aCBTypeNumber, int? noOfDigits, string weightings, int? modulus, int? fudgeFactor, string exceptionCode, string userID, DateTime dateChange)
        {
            this.ACBBankCode = aCBBankCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.NoOfDigits = noOfDigits;
            this.Weightings = weightings;
            this.Modulus = modulus;
            this.FudgeFactor = fudgeFactor;
            this.ExceptionCode = exceptionCode;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }
		[JsonConstructor]
        public CDVExceptionsDataModel(int cDVExceptionsKey, int aCBBankCode, int aCBTypeNumber, int? noOfDigits, string weightings, int? modulus, int? fudgeFactor, string exceptionCode, string userID, DateTime dateChange)
        {
            this.CDVExceptionsKey = cDVExceptionsKey;
            this.ACBBankCode = aCBBankCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.NoOfDigits = noOfDigits;
            this.Weightings = weightings;
            this.Modulus = modulus;
            this.FudgeFactor = fudgeFactor;
            this.ExceptionCode = exceptionCode;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }		

        public int CDVExceptionsKey { get; set; }

        public int ACBBankCode { get; set; }

        public int ACBTypeNumber { get; set; }

        public int? NoOfDigits { get; set; }

        public string Weightings { get; set; }

        public int? Modulus { get; set; }

        public int? FudgeFactor { get; set; }

        public string ExceptionCode { get; set; }

        public string UserID { get; set; }

        public DateTime DateChange { get; set; }

        public void SetKey(int key)
        {
            this.CDVExceptionsKey =  key;
        }
    }
}