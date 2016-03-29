using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BondDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BondDataModel(int deedsOfficeKey, int attorneyKey, string bondRegistrationNumber, DateTime bondRegistrationDate, double bondRegistrationAmount, double bondLoanAgreementAmount, string userID, DateTime? changeDate, int? offerKey)
        {
            this.DeedsOfficeKey = deedsOfficeKey;
            this.AttorneyKey = attorneyKey;
            this.BondRegistrationNumber = bondRegistrationNumber;
            this.BondRegistrationDate = bondRegistrationDate;
            this.BondRegistrationAmount = bondRegistrationAmount;
            this.BondLoanAgreementAmount = bondLoanAgreementAmount;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.OfferKey = offerKey;
		
        }
		[JsonConstructor]
        public BondDataModel(int bondKey, int deedsOfficeKey, int attorneyKey, string bondRegistrationNumber, DateTime bondRegistrationDate, double bondRegistrationAmount, double bondLoanAgreementAmount, string userID, DateTime? changeDate, int? offerKey)
        {
            this.BondKey = bondKey;
            this.DeedsOfficeKey = deedsOfficeKey;
            this.AttorneyKey = attorneyKey;
            this.BondRegistrationNumber = bondRegistrationNumber;
            this.BondRegistrationDate = bondRegistrationDate;
            this.BondRegistrationAmount = bondRegistrationAmount;
            this.BondLoanAgreementAmount = bondLoanAgreementAmount;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.OfferKey = offerKey;
		
        }		

        public int BondKey { get; set; }

        public int DeedsOfficeKey { get; set; }

        public int AttorneyKey { get; set; }

        public string BondRegistrationNumber { get; set; }

        public DateTime BondRegistrationDate { get; set; }

        public double BondRegistrationAmount { get; set; }

        public double BondLoanAgreementAmount { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? OfferKey { get; set; }

        public void SetKey(int key)
        {
            this.BondKey =  key;
        }
    }
}