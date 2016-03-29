using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportLegalEntityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportLegalEntityDataModel(int offerKey, string maritalStatusKey, string genderKey, string citizenTypeKey, string salutationKey, string firstNames, string initials, string surname, string preferredName, string iDNumber, string passportNumber, string taxNumber, string homePhoneCode, string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string cellPhoneNumber, string emailAddress, string faxCode, string faxNumber, int? importID)
        {
            this.OfferKey = offerKey;
            this.MaritalStatusKey = maritalStatusKey;
            this.GenderKey = genderKey;
            this.CitizenTypeKey = citizenTypeKey;
            this.SalutationKey = salutationKey;
            this.FirstNames = firstNames;
            this.Initials = initials;
            this.Surname = surname;
            this.PreferredName = preferredName;
            this.IDNumber = iDNumber;
            this.PassportNumber = passportNumber;
            this.TaxNumber = taxNumber;
            this.HomePhoneCode = homePhoneCode;
            this.HomePhoneNumber = homePhoneNumber;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhoneNumber = workPhoneNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.ImportID = importID;
		
        }
		[JsonConstructor]
        public ImportLegalEntityDataModel(int legalEntityKey, int offerKey, string maritalStatusKey, string genderKey, string citizenTypeKey, string salutationKey, string firstNames, string initials, string surname, string preferredName, string iDNumber, string passportNumber, string taxNumber, string homePhoneCode, string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string cellPhoneNumber, string emailAddress, string faxCode, string faxNumber, int? importID)
        {
            this.LegalEntityKey = legalEntityKey;
            this.OfferKey = offerKey;
            this.MaritalStatusKey = maritalStatusKey;
            this.GenderKey = genderKey;
            this.CitizenTypeKey = citizenTypeKey;
            this.SalutationKey = salutationKey;
            this.FirstNames = firstNames;
            this.Initials = initials;
            this.Surname = surname;
            this.PreferredName = preferredName;
            this.IDNumber = iDNumber;
            this.PassportNumber = passportNumber;
            this.TaxNumber = taxNumber;
            this.HomePhoneCode = homePhoneCode;
            this.HomePhoneNumber = homePhoneNumber;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhoneNumber = workPhoneNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.ImportID = importID;
		
        }		

        public int LegalEntityKey { get; set; }

        public int OfferKey { get; set; }

        public string MaritalStatusKey { get; set; }

        public string GenderKey { get; set; }

        public string CitizenTypeKey { get; set; }

        public string SalutationKey { get; set; }

        public string FirstNames { get; set; }

        public string Initials { get; set; }

        public string Surname { get; set; }

        public string PreferredName { get; set; }

        public string IDNumber { get; set; }

        public string PassportNumber { get; set; }

        public string TaxNumber { get; set; }

        public string HomePhoneCode { get; set; }

        public string HomePhoneNumber { get; set; }

        public string WorkPhoneCode { get; set; }

        public string WorkPhoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string FaxCode { get; set; }

        public string FaxNumber { get; set; }

        public int? ImportID { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityKey =  key;
        }
    }
}