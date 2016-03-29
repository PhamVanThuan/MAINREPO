using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditLegalEntityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditLegalEntityDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityKey, int? legalEntityTypeKey, int? maritalStatusKey, int? genderKey, int? populationGroupKey, DateTime introductionDate, string firstNames, string initials, string surname, string preferredName, string iDNumber, string passportNumber, string taxNumber, string registrationNumber, string registeredName, string tradingName, DateTime? dateOfBirth, string homePhoneCode, string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string cellPhoneNumber, string emailAddress, string faxCode, string faxNumber, string password, int? salutationKey, int? citizenTypeKey, int? legalEntityStatusKey, string comments, int? legalEntityExceptionStatusKey, string userID, DateTime? changeDate, int? educationKey, int? homeLanguageKey, int? documentLanguageKey, int? residenceStatusKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityKey = legalEntityKey;
            this.LegalEntityTypeKey = legalEntityTypeKey;
            this.MaritalStatusKey = maritalStatusKey;
            this.GenderKey = genderKey;
            this.PopulationGroupKey = populationGroupKey;
            this.IntroductionDate = introductionDate;
            this.FirstNames = firstNames;
            this.Initials = initials;
            this.Surname = surname;
            this.PreferredName = preferredName;
            this.IDNumber = iDNumber;
            this.PassportNumber = passportNumber;
            this.TaxNumber = taxNumber;
            this.RegistrationNumber = registrationNumber;
            this.RegisteredName = registeredName;
            this.TradingName = tradingName;
            this.DateOfBirth = dateOfBirth;
            this.HomePhoneCode = homePhoneCode;
            this.HomePhoneNumber = homePhoneNumber;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhoneNumber = workPhoneNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.Password = password;
            this.SalutationKey = salutationKey;
            this.CitizenTypeKey = citizenTypeKey;
            this.LegalEntityStatusKey = legalEntityStatusKey;
            this.Comments = comments;
            this.LegalEntityExceptionStatusKey = legalEntityExceptionStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.EducationKey = educationKey;
            this.HomeLanguageKey = homeLanguageKey;
            this.DocumentLanguageKey = documentLanguageKey;
            this.ResidenceStatusKey = residenceStatusKey;
		
        }
		[JsonConstructor]
        public AuditLegalEntityDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityKey, int? legalEntityTypeKey, int? maritalStatusKey, int? genderKey, int? populationGroupKey, DateTime introductionDate, string firstNames, string initials, string surname, string preferredName, string iDNumber, string passportNumber, string taxNumber, string registrationNumber, string registeredName, string tradingName, DateTime? dateOfBirth, string homePhoneCode, string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string cellPhoneNumber, string emailAddress, string faxCode, string faxNumber, string password, int? salutationKey, int? citizenTypeKey, int? legalEntityStatusKey, string comments, int? legalEntityExceptionStatusKey, string userID, DateTime? changeDate, int? educationKey, int? homeLanguageKey, int? documentLanguageKey, int? residenceStatusKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityKey = legalEntityKey;
            this.LegalEntityTypeKey = legalEntityTypeKey;
            this.MaritalStatusKey = maritalStatusKey;
            this.GenderKey = genderKey;
            this.PopulationGroupKey = populationGroupKey;
            this.IntroductionDate = introductionDate;
            this.FirstNames = firstNames;
            this.Initials = initials;
            this.Surname = surname;
            this.PreferredName = preferredName;
            this.IDNumber = iDNumber;
            this.PassportNumber = passportNumber;
            this.TaxNumber = taxNumber;
            this.RegistrationNumber = registrationNumber;
            this.RegisteredName = registeredName;
            this.TradingName = tradingName;
            this.DateOfBirth = dateOfBirth;
            this.HomePhoneCode = homePhoneCode;
            this.HomePhoneNumber = homePhoneNumber;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhoneNumber = workPhoneNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.Password = password;
            this.SalutationKey = salutationKey;
            this.CitizenTypeKey = citizenTypeKey;
            this.LegalEntityStatusKey = legalEntityStatusKey;
            this.Comments = comments;
            this.LegalEntityExceptionStatusKey = legalEntityExceptionStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.EducationKey = educationKey;
            this.HomeLanguageKey = homeLanguageKey;
            this.DocumentLanguageKey = documentLanguageKey;
            this.ResidenceStatusKey = residenceStatusKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int LegalEntityKey { get; set; }

        public int? LegalEntityTypeKey { get; set; }

        public int? MaritalStatusKey { get; set; }

        public int? GenderKey { get; set; }

        public int? PopulationGroupKey { get; set; }

        public DateTime IntroductionDate { get; set; }

        public string FirstNames { get; set; }

        public string Initials { get; set; }

        public string Surname { get; set; }

        public string PreferredName { get; set; }

        public string IDNumber { get; set; }

        public string PassportNumber { get; set; }

        public string TaxNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public string RegisteredName { get; set; }

        public string TradingName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string HomePhoneCode { get; set; }

        public string HomePhoneNumber { get; set; }

        public string WorkPhoneCode { get; set; }

        public string WorkPhoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string FaxCode { get; set; }

        public string FaxNumber { get; set; }

        public string Password { get; set; }

        public int? SalutationKey { get; set; }

        public int? CitizenTypeKey { get; set; }

        public int? LegalEntityStatusKey { get; set; }

        public string Comments { get; set; }

        public int? LegalEntityExceptionStatusKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? EducationKey { get; set; }

        public int? HomeLanguageKey { get; set; }

        public int? DocumentLanguageKey { get; set; }

        public int? ResidenceStatusKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}