using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportOfferDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportOfferDataModel(int fileKey, int importStatusKey, double? offerAmount, DateTime? offerStartDate, DateTime? offerEndDate, string mortgageLoanPurposeKey, string applicantTypeKey, int? numberApplicants, DateTime? homePurchaseDate, DateTime? bondRegistrationDate, double? currentBondValue, DateTime? deedsOfficeDate, string bondFinancialInstitution, double? existingLoan, double? purchasePrice, string reference, string errorMsg, int? importID)
        {
            this.FileKey = fileKey;
            this.ImportStatusKey = importStatusKey;
            this.OfferAmount = offerAmount;
            this.OfferStartDate = offerStartDate;
            this.OfferEndDate = offerEndDate;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.ApplicantTypeKey = applicantTypeKey;
            this.NumberApplicants = numberApplicants;
            this.HomePurchaseDate = homePurchaseDate;
            this.BondRegistrationDate = bondRegistrationDate;
            this.CurrentBondValue = currentBondValue;
            this.DeedsOfficeDate = deedsOfficeDate;
            this.BondFinancialInstitution = bondFinancialInstitution;
            this.ExistingLoan = existingLoan;
            this.PurchasePrice = purchasePrice;
            this.Reference = reference;
            this.ErrorMsg = errorMsg;
            this.ImportID = importID;
		
        }
		[JsonConstructor]
        public ImportOfferDataModel(int offerKey, int fileKey, int importStatusKey, double? offerAmount, DateTime? offerStartDate, DateTime? offerEndDate, string mortgageLoanPurposeKey, string applicantTypeKey, int? numberApplicants, DateTime? homePurchaseDate, DateTime? bondRegistrationDate, double? currentBondValue, DateTime? deedsOfficeDate, string bondFinancialInstitution, double? existingLoan, double? purchasePrice, string reference, string errorMsg, int? importID)
        {
            this.OfferKey = offerKey;
            this.FileKey = fileKey;
            this.ImportStatusKey = importStatusKey;
            this.OfferAmount = offerAmount;
            this.OfferStartDate = offerStartDate;
            this.OfferEndDate = offerEndDate;
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.ApplicantTypeKey = applicantTypeKey;
            this.NumberApplicants = numberApplicants;
            this.HomePurchaseDate = homePurchaseDate;
            this.BondRegistrationDate = bondRegistrationDate;
            this.CurrentBondValue = currentBondValue;
            this.DeedsOfficeDate = deedsOfficeDate;
            this.BondFinancialInstitution = bondFinancialInstitution;
            this.ExistingLoan = existingLoan;
            this.PurchasePrice = purchasePrice;
            this.Reference = reference;
            this.ErrorMsg = errorMsg;
            this.ImportID = importID;
		
        }		

        public int OfferKey { get; set; }

        public int FileKey { get; set; }

        public int ImportStatusKey { get; set; }

        public double? OfferAmount { get; set; }

        public DateTime? OfferStartDate { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public string MortgageLoanPurposeKey { get; set; }

        public string ApplicantTypeKey { get; set; }

        public int? NumberApplicants { get; set; }

        public DateTime? HomePurchaseDate { get; set; }

        public DateTime? BondRegistrationDate { get; set; }

        public double? CurrentBondValue { get; set; }

        public DateTime? DeedsOfficeDate { get; set; }

        public string BondFinancialInstitution { get; set; }

        public double? ExistingLoan { get; set; }

        public double? PurchasePrice { get; set; }

        public string Reference { get; set; }

        public string ErrorMsg { get; set; }

        public int? ImportID { get; set; }

        public void SetKey(int key)
        {
            this.OfferKey =  key;
        }
    }
}