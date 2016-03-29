using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditBondDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditBondDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int bondKey, int deedsOfficeKey, int attorneyKey, string bondRegistrationNumber, DateTime? bondRegistrationDate, double? bondRegistrationAmount, double? bondLoanAgreementAmount, string userID, DateTime? changeDate, int? offerKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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
		[JsonConstructor]
        public AuditBondDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int bondKey, int deedsOfficeKey, int attorneyKey, string bondRegistrationNumber, DateTime? bondRegistrationDate, double? bondRegistrationAmount, double? bondLoanAgreementAmount, string userID, DateTime? changeDate, int? offerKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int BondKey { get; set; }

        public int DeedsOfficeKey { get; set; }

        public int AttorneyKey { get; set; }

        public string BondRegistrationNumber { get; set; }

        public DateTime? BondRegistrationDate { get; set; }

        public double? BondRegistrationAmount { get; set; }

        public double? BondLoanAgreementAmount { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? OfferKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}