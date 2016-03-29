using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public class LegalEntity :Record ,IComparable<LegalEntity>
    {
        #region Keys

        public int LegalEntityKey { get; set; }

        public LegalEntityTypeEnum LegalEntityTypeKey { get; set; }

        public SalutationTypeEnum? SalutationKey { get; set; }

        public GenderTypeEnum GenderKey { get; set; }

        public MaritalStatusEnum MaritalStatusKey { get; set; }

        public PopulationGroupEnum PopulationGroupKey { get; set; }

        public EducationEnum EducationKey { get; set; }

        public CitizenTypeEnum CitizenTypeKey { get; set; }

        public LanguageEnum HomeLanguageKey { get; set; }

        public LanguageEnum DocumentLanguageKey { get; set; }

        public LegalEntityStatusEnum LegalEntityStatusKey { get; set; }

        public RoleTypeEnum RoleTypeKey { get; set; }

        #endregion Keys

        #region Descriptions

        public string LegalEntityTypeDescription { get; set; }

        public string MaritalStatusDescription { get; set; }

        public string GenderDescription { get; set; }

        public string PopulationGroupDescription { get; set; }

        public string SalutationDescription { get; set; }

        public string CitizenTypeDescription { get; set; }

        public string LegalEntityStatusDescription { get; set; }

        public string EducationDescription { get; set; }

        public string HomeLanguageDescription { get; set; }

        public string DocumentLanguageDescription { get; set; }

        #endregion Descriptions

        public bool IncomeContributor { get; set; }

        public string IdNumber { get; set; }

        public string Initials { get; set; }

        public string FirstNames { get; set; }

        public string Surname { get; set; }

        public string PreferredName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PassportNumber { get; set; }

        public string TaxNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public string RegisteredName { get; set; }

        public string TradingName { get; set; }

        public string HomePhoneCode { get; set; }

        public string HomePhoneNumber { get; set; }

        public string WorkPhoneCode { get; set; }

        public string WorkPhoneNumber { get; set; }

        public string FaxCode { get; set; }

        public string FaxNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public bool TeleMarketing { get; set; }

        public bool Marketing { get; set; }

        public bool CustomerList { get; set; }

        public bool Email { get; set; }

        public bool Sms { get; set; }

        public DateTime? IntroductionDate { get; set; }

        public LegalEntityExceptionStatusEnum LegalEntityExceptionStatusKey { get; set; }

        public string Password { get; set; }

        public string Comments { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public ResidenceStatusEnum ResidenceStatusKey { get; set; }

        public LegalEntityAddress LegalEntityAddress { get; set; }

        public List<string> CompareResults { get; set; }

        public int CompareTo(LegalEntity other)
        {
            this.CompareResults = new List<string>();
            if (this.IntroductionDate.Value.Date != other.IntroductionDate.Value.Date)
                this.CompareResults.Add("IntroductionDate mismatch");
            if (this.RegisteredName != other.RegisteredName)
                this.CompareResults.Add("RegisteredName mismatch");
            if (this.RegistrationNumber != other.RegistrationNumber)
                this.CompareResults.Add("RegistrationNumber mismatch");
            if (this.TradingName != other.TradingName)
                this.CompareResults.Add("TradingName mismatch");
            if (this.DocumentLanguageDescription != other.DocumentLanguageDescription)
                this.CompareResults.Add("DocumentLanguageDescription mismatch");
            if (this.IdNumber != other.IdNumber)
                this.CompareResults.Add("IdNumber mismatch");
            if (this.Initials != other.Initials)
                this.CompareResults.Add("Initials mismatch");
            if (this.FirstNames != other.FirstNames)
                this.CompareResults.Add("FirstNames mismatch");
            if (this.Surname != other.Surname)
                this.CompareResults.Add("Surname mismatch");
            if (this.PreferredName != other.PreferredName)
                this.CompareResults.Add("PreferredName mismatch");
            if (this.DateOfBirth != other.DateOfBirth)
                this.CompareResults.Add("DateOfBirth mismatch");
            if (this.PassportNumber != other.PassportNumber)
                this.CompareResults.Add("PassportNumber mismatch");
            if (this.TaxNumber != other.TaxNumber)
                this.CompareResults.Add("TaxNumber mismatch");
            if (this.HomePhoneCode != other.HomePhoneCode)
                this.CompareResults.Add("HomePhoneCode mismatch");
            if (this.HomePhoneNumber != other.HomePhoneNumber)
                this.CompareResults.Add("HomePhoneNumber mismatch");
            if (this.WorkPhoneCode != other.WorkPhoneCode)
                this.CompareResults.Add("IntroductionDate mismatch");
            if (this.WorkPhoneNumber != other.WorkPhoneNumber)
                this.CompareResults.Add("WorkPhoneNumber mismatch");
            if (this.FaxCode != other.FaxCode)
                this.CompareResults.Add("FaxCode mismatch");
            if (this.FaxNumber != other.FaxNumber)
                this.CompareResults.Add("FaxNumber mismatch");
            if (this.CellPhoneNumber != other.CellPhoneNumber)
                this.CompareResults.Add("CellPhoneNumber mismatch");
            if (this.EmailAddress != other.EmailAddress)
                this.CompareResults.Add("EmailAddress mismatch");
            if (this.UserID != other.UserID)
                this.CompareResults.Add("UserID mismatch");
            if (this.ChangeDate.Value.Date != other.ChangeDate.Value.Date)
                this.CompareResults.Add("ChangeDate mismatch");
            if (this.CompareResults.Count > 0)
                return 0;
            return 1;
        }

        public LegalEntity Copy()
        {
            var copyInstance = new LegalEntity();
            var thisInstance = this;
            Helpers.SetProperties<LegalEntity, LegalEntity>(ref copyInstance, thisInstance);
            return copyInstance;
        }
    }
}