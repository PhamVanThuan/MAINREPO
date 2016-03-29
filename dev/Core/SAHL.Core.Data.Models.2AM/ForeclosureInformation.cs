using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ForeclosureInformationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ForeclosureInformationDataModel(int? foreclosureKey, int? foreclosureOutcomeKey, int? attorneyKey, int? accountKey, int? sPVKey, double? currentBalance, double? lTV, int? monthsInArrears, DateTime? foreclosureInformationDateTime)
        {
            this.ForeclosureKey = foreclosureKey;
            this.ForeclosureOutcomeKey = foreclosureOutcomeKey;
            this.AttorneyKey = attorneyKey;
            this.AccountKey = accountKey;
            this.SPVKey = sPVKey;
            this.CurrentBalance = currentBalance;
            this.LTV = lTV;
            this.MonthsInArrears = monthsInArrears;
            this.ForeclosureInformationDateTime = foreclosureInformationDateTime;
		
        }
		[JsonConstructor]
        public ForeclosureInformationDataModel(int foreclosureInformationKey, int? foreclosureKey, int? foreclosureOutcomeKey, int? attorneyKey, int? accountKey, int? sPVKey, double? currentBalance, double? lTV, int? monthsInArrears, DateTime? foreclosureInformationDateTime)
        {
            this.ForeclosureInformationKey = foreclosureInformationKey;
            this.ForeclosureKey = foreclosureKey;
            this.ForeclosureOutcomeKey = foreclosureOutcomeKey;
            this.AttorneyKey = attorneyKey;
            this.AccountKey = accountKey;
            this.SPVKey = sPVKey;
            this.CurrentBalance = currentBalance;
            this.LTV = lTV;
            this.MonthsInArrears = monthsInArrears;
            this.ForeclosureInformationDateTime = foreclosureInformationDateTime;
		
        }		

        public int ForeclosureInformationKey { get; set; }

        public int? ForeclosureKey { get; set; }

        public int? ForeclosureOutcomeKey { get; set; }

        public int? AttorneyKey { get; set; }

        public int? AccountKey { get; set; }

        public int? SPVKey { get; set; }

        public double? CurrentBalance { get; set; }

        public double? LTV { get; set; }

        public int? MonthsInArrears { get; set; }

        public DateTime? ForeclosureInformationDateTime { get; set; }

        public void SetKey(int key)
        {
            this.ForeclosureInformationKey =  key;
        }
    }
}