using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class NaturalPersonClientModel : ValidatableModel, INaturalPersonClientModel
    {
        public NaturalPersonClientModel(string idNumber, string passportNumber, SalutationType? salutation, string firstName, string surname, string initials, string preferredName, Gender? gender, 
            MaritalStatus? maritalStatus, PopulationGroup? populationGroup, CitizenType? citizenshipType, DateTime? dateOfBirth, Language? homeLanguage, 
            CorrespondenceLanguage? correspondenceLanguage, Education? education, string homePhoneCode, string homePhone, string workPhoneCode, string workPhone, string faxCode, 
            string faxNumber, string cellphone, string emailAddress)
        {
            this.IDNumber = idNumber;
            this.PassportNumber = passportNumber;
            this.Salutation = salutation;
            this.FirstName = firstName;
            this.Surname = surname;
            this.Initials = initials;
            this.PreferredName = preferredName;
            this.Gender = gender;
            this.MaritalStatus = maritalStatus;
            this.PopulationGroup = populationGroup;
            this.CitizenshipType = citizenshipType;
            this.DateOfBirth = dateOfBirth;
            this.HomeLanguage = homeLanguage;
            this.CorrespondenceLanguage = correspondenceLanguage;
            this.Education = education;
            this.HomePhoneCode = homePhoneCode;
            this.HomePhone = homePhone;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhone = workPhone;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.Cellphone = cellphone;
            this.EmailAddress = emailAddress;
            Validate();
        }

        public string IDNumber { get; protected set; }

        public string PassportNumber { get; protected set; }

        public SalutationType? Salutation { get; protected set; }

        [Required]
        public string FirstName { get; protected set; }

        [Required]
        public string Surname { get; protected set; }

        public string Initials { get; protected set; }

        public string PreferredName { get; protected set; }

        public Gender? Gender { get; protected set; }

        public MaritalStatus? MaritalStatus { get; protected set; }

        public PopulationGroup? PopulationGroup { get; protected set; }

        public CitizenType? CitizenshipType { get; protected set; }

        public DateTime? DateOfBirth { get; protected set; }

        public Language? HomeLanguage { get; protected set; }

        public CorrespondenceLanguage? CorrespondenceLanguage { get; protected set; }

        public Education? Education { get; protected set; }

        public string HomePhoneCode { get; protected set; }

        public string HomePhone { get; protected set; }

        public string WorkPhoneCode { get; protected set; }

        public string WorkPhone { get; protected set; }

        public string FaxCode { get; protected set; }

        public string FaxNumber { get; protected set; }

        public string Cellphone { get; protected set; }

        public string EmailAddress { get; protected set; }
    }
}