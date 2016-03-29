using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantModel : IDataModel
    {
        private IEnumerable<BankAccountModel> bankAccounts;
        private IEnumerable<EmploymentModel> employments;
        private IEnumerable<AddressModel> addresses;
        private IEnumerable<ApplicantAffordabilityModel> affordabilities;
        private IEnumerable<ApplicantDeclarationsModel> applicantDeclarations;
        private IEnumerable<ApplicantAssetLiabilityModel> applicantAssetLiabilities;
        private IEnumerable<ApplicantMarketingOptionModel> applicantMarketingOptions;

        public ApplicantModel(string idNumber, string passportNumber, SalutationType salutation, string firstName, string surname, string initials,
            string preferredName,
            Gender gender, MaritalStatus maritalStatus, PopulationGroup populationGroup, CitizenType citizenshipType, DateTime dateOfBirth,
            Language homeLanguage, CorrespondenceLanguage correspondenceLanguage, string homePhoneCode, string homePhone, string workPhoneCode,
            string workPhone, string cellPhone, string emailAddress, string faxCode, string faxNumber,
            LeadApplicantOfferRoleTypeEnum applicantRoleType,
            List<BankAccountModel> bankAccounts, List<EmploymentModel> employments, List<AddressModel> addresses,
            List<ApplicantAffordabilityModel> affordabilities, List<ApplicantDeclarationsModel> applicantDeclarations,
            IEnumerable<ApplicantAssetLiabilityModel> applicantAssetLiabilities, IEnumerable<ApplicantMarketingOptionModel> applicantMarketingOptions,
            Education education, bool incomeContributor)
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
            this.HomePhoneCode = homePhoneCode;
            this.HomePhone = homePhone;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhone = workPhone;
            this.CellPhone = cellPhone;
            this.EmailAddress = emailAddress;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.ApplicantRoleType = applicantRoleType;
            this.BankAccounts = bankAccounts;
            this.Employments = employments;
            this.Addresses = addresses;
            this.Affordabilities = affordabilities;
            this.ApplicantDeclarations = applicantDeclarations;
            this.ApplicantAssetLiabilities = applicantAssetLiabilities;
            this.ApplicantMarketingOptions = applicantMarketingOptions;
            this.Education = education;
            this.IncomeContributor = incomeContributor;
        }

        [DataMember]
        public string IDNumber { get; set; }

        [DataMember]
        public string PassportNumber { get; set; }

        [DataMember]
        public SalutationType Salutation { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string Initials { get; set; }

        [DataMember]
        public string PreferredName { get; set; }

        [DataMember]
        public Gender Gender { get; set; }

        [DataMember]
        public MaritalStatus MaritalStatus { get; set; }

        [DataMember]
        public PopulationGroup PopulationGroup { get; set; }

        [DataMember]
        public CitizenType CitizenshipType { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public Language HomeLanguage { get; set; }

        [DataMember]
        public CorrespondenceLanguage CorrespondenceLanguage { get; set; }

        [DataMember]
        public string HomePhoneCode { get; set; }

        [DataMember]
        public string HomePhone { get; set; }

        [DataMember]
        public string WorkPhoneCode { get; set; }

        [DataMember]
        public string WorkPhone { get; set; }

        [DataMember]
        public string CellPhone { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string FaxCode { get; set; }

        [DataMember]
        public string FaxNumber { get; set; }

        [DataMember]
        public LeadApplicantOfferRoleTypeEnum ApplicantRoleType { get; set; }

        [DataMember]
        public IEnumerable<BankAccountModel> BankAccounts
        {
            get { return bankAccounts; }
            set
            {
                if (value != null)
                {
                    bankAccounts = new List<BankAccountModel>(value);
                }
            }
        }

        [DataMember]
        public IEnumerable<EmploymentModel> Employments
        {
            get { return employments; }
            set
            {
                if (value != null)
                {
                    employments = new List<EmploymentModel>(value);
                }
            }
        }

        [DataMember]
        public IEnumerable<AddressModel> Addresses
        {
            get { return addresses; }
            set
            {
                if (value != null)
                {
                    addresses = new List<AddressModel>(value);
                }
            }
        }

        [DataMember]
        public IEnumerable<ApplicantAffordabilityModel> Affordabilities
        {
            get { return affordabilities; }
            set
            {
                if (value != null)
                {
                    affordabilities = new List<ApplicantAffordabilityModel>(value);
                }
            }
        }

        [DataMember]
        public IEnumerable<ApplicantDeclarationsModel> ApplicantDeclarations
        {
            get { return applicantDeclarations; }
            set
            {
                if (value != null)
                {
                    applicantDeclarations = new List<ApplicantDeclarationsModel>(value);
                }
            }
        }

        [DataMember]
        public IEnumerable<ApplicantAssetLiabilityModel> ApplicantAssetLiabilities
        {
            get { return applicantAssetLiabilities; }
            set
            {
                if (value != null)
                {
                    applicantAssetLiabilities = new List<ApplicantAssetLiabilityModel>(value);
                }
            }
        }

        [DataMember]
        public IEnumerable<ApplicantMarketingOptionModel> ApplicantMarketingOptions
        {
            get { return applicantMarketingOptions; }
            set
            {
                if (value != null)
                {
                    applicantMarketingOptions = new List<ApplicantMarketingOptionModel>(value);
                }
            }
        }

        [DataMember]
        public Education Education { get; set; }

        [DataMember]
        public bool IncomeContributor { get; set; }
    }
}
