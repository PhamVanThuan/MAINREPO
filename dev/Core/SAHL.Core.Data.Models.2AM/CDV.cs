using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CDVDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CDVDataModel(int aCBBankCode, string aCBBranchCode, int aCBTypeNumber, int? streamCode, int? exceptionStreamCode, string weightings, int? modulus, int? fudgeFactor, string exceptionCode, int? accountIndicator, string userID, DateTime dateChange)
        {
            this.ACBBankCode = aCBBankCode;
            this.ACBBranchCode = aCBBranchCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.StreamCode = streamCode;
            this.ExceptionStreamCode = exceptionStreamCode;
            this.Weightings = weightings;
            this.Modulus = modulus;
            this.FudgeFactor = fudgeFactor;
            this.ExceptionCode = exceptionCode;
            this.AccountIndicator = accountIndicator;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }
		[JsonConstructor]
        public CDVDataModel(int cDVKey, int aCBBankCode, string aCBBranchCode, int aCBTypeNumber, int? streamCode, int? exceptionStreamCode, string weightings, int? modulus, int? fudgeFactor, string exceptionCode, int? accountIndicator, string userID, DateTime dateChange)
        {
            this.CDVKey = cDVKey;
            this.ACBBankCode = aCBBankCode;
            this.ACBBranchCode = aCBBranchCode;
            this.ACBTypeNumber = aCBTypeNumber;
            this.StreamCode = streamCode;
            this.ExceptionStreamCode = exceptionStreamCode;
            this.Weightings = weightings;
            this.Modulus = modulus;
            this.FudgeFactor = fudgeFactor;
            this.ExceptionCode = exceptionCode;
            this.AccountIndicator = accountIndicator;
            this.UserID = userID;
            this.DateChange = dateChange;
		
        }		

        public int CDVKey { get; set; }

        public int ACBBankCode { get; set; }

        public string ACBBranchCode { get; set; }

        public int ACBTypeNumber { get; set; }

        public int? StreamCode { get; set; }

        public int? ExceptionStreamCode { get; set; }

        public string Weightings { get; set; }

        public int? Modulus { get; set; }

        public int? FudgeFactor { get; set; }

        public string ExceptionCode { get; set; }

        public int? AccountIndicator { get; set; }

        public string UserID { get; set; }

        public DateTime DateChange { get; set; }

        public void SetKey(int key)
        {
            this.CDVKey =  key;
        }
    }
}