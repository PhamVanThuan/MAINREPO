using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class ActiveNaturalPersonClientModel : ValidatableModel, IClientContactDetails
    {
        public ActiveNaturalPersonClientModel(SalutationType salutation, string preferredName, Language homeLanguage, CorrespondenceLanguage correspondenceLanguage, Education education, 
            string homePhoneCode, string homePhone, string workPhoneCode, string workPhone, string faxCode, string faxNumber, string cellphone, string emailAddress, DateTime? dateOfBirth=null)
        {
            this.Salutation = salutation;
            this.PreferredName = preferredName;
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
            this.DateOfBirth = dateOfBirth;
            Validate();
        }

        [Required]
        public SalutationType Salutation { get; protected set; }

        public string PreferredName { get; protected set; }

        [Required]
        public Language HomeLanguage { get; protected set; }

        [Required]
        public CorrespondenceLanguage CorrespondenceLanguage { get; protected set; }

        public Education? Education { get; protected set; }

        public string HomePhoneCode { get; protected set; }

        public string HomePhone { get; protected set; }

        public string WorkPhoneCode { get; protected set; }

        public string WorkPhone { get; protected set; }

        public string FaxCode { get; protected set; }

        public string FaxNumber { get; protected set; }

        public string Cellphone { get; protected set; }

        public string EmailAddress { get; protected set; }

        public DateTime? DateOfBirth { get; protected set; }
    }
}