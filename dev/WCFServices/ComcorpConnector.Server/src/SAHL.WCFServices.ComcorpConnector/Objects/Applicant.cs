using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class Applicant
    {
        [DataMember]
        public string AddressCityWork { get; set; }

        [DataMember]
        public string AddressLine1Work { get; set; }

        [DataMember]
        public string AddressSuburbWork { get; set; }

        [DataMember]
        public DateTime AdminRescinded { get; set; }

        [DataMember]
        public bool BusinessMember { get; set; }

        [DataMember]
        public string Cellphone { get; set; }

        [DataMember]
        [Required]
        public string CorrespondenceLanguage { get; set; }

        [DataMember]
        [Required]
        public string CurrentEmploymentType { get; set; }

        [DataMember]
        [Required]
        public string EmploymentSector { get; set; }

        [DataMember]
        [Required]
        public string EmployerBusinessType { get; set; }

        [DataMember]
        public Decimal EmployerGrossMonthlySalary { get; set; }

        [DataMember]
        public Decimal EmployeeUIF { get; set; }

        [DataMember]
        public Decimal EmployeePAYE { get; set; }

        [DataMember]
        public Decimal EmployeeMedicalAid { get; set; }

        [DataMember]
        public Decimal EmployeePension { get; set; }

        [DataMember]
        public string EmployerRemunerationType { get; set; }

        [DataMember]
        public DateTime DateJoinedEmployer { get; set; }

        [DataMember]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public DateTime DateRehabilitated { get; set; }

        [DataMember]
        public int DateSalaryPaid { get; set; }

        [DataMember]
        public bool DebtRearrangement { get; set; }

        [DataMember]
        public bool DeclaredInsolvent { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string EmployeeNo { get; set; }

        [DataMember]
        public string EmployerEmail { get; set; }

        [DataMember]
        public string EmployerName { get; set; }

        [DataMember]
        [Required]
        public string EthnicGroup { get; set; }

        [DataMember]
        public string FaxCode { get; set; }

        [DataMember]
        public string FaxNo { get; set; }

        [DataMember]
        [Required]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        public string Gender { get; set; }

        [DataMember]
        [Required]
        public string HomeLanguage { get; set; }

        [DataMember]
        public string HomePhone { get; set; }

        [DataMember]
        public string HomePhoneCode { get; set; }

        [DataMember]
        public string IdentificationNo { get; set; }

        [DataMember]
        public bool IncomeContributor { get; set; }

        [DataMember]
        public bool IsUnderAdminOrder { get; set; }

        [DataMember]
        public bool IsUnderDebtReview { get; set; }

        [DataMember]
        public bool JudgementIndicator { get; set; }

        [DataMember]
        public string JudgementValue { get; set; }

        [DataMember]
        [Required]
        public string MaritalStatus { get; set; }

        [DataMember]
        public bool MarketingConsumerLists { get; set; }

        [DataMember]
        public bool MarketingEmail { get; set; }

        [DataMember]
        public bool MarketingFlyer { get; set; }

        [DataMember]
        public bool MarketingMagazine { get; set; }

        [DataMember]
        public bool MarketingMarketing { get; set; }

        [DataMember]
        public bool MarketingNewspaper { get; set; }

        [DataMember]
        public bool MarketingOnline { get; set; }

        [DataMember]
        public bool MarketingOther { get; set; }

        [DataMember]
        public bool MarketingRadio { get; set; }

        [DataMember]
        public bool MarketingReferral { get; set; }

        [DataMember]
        public bool MarketingSMS { get; set; }

        [DataMember]
        public bool MarketingTelemarketing { get; set; }

        [DataMember]
        public bool MarketingTV { get; set; }

        [DataMember]
        public string MarriedStatus { get; set; }

        [DataMember]
        public bool MayDoCreditBureauEnquiry { get; set; }

        [DataMember]
        public string MemberCompanyName { get; set; }

        [DataMember]
        public string MemberCompanyNumber { get; set; }

        [DataMember]
        public string NetAssets { get; set; }

        [DataMember]
        public string Occupation { get; set; }

        [DataMember]
        public string PassportNo { get; set; }

        [DataMember]
        public bool PermanentResident { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Physical Address City cannot be longer than 50 characters.")]
        public string PhysicalAddressCity { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Physical Address Code cannot be longer than 50 characters.")]
        public string PhysicalAddressCode { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Physical Address Line 1 cannot be longer than 50 characters.")]
        public string PhysicalAddressLine1 { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Physical Address Line 2 cannot be longer than 50 characters.")]
        public string PhysicalAddressLine2 { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Physical Address Country cannot be longer than 50 characters.")]
        public string PhysicalCountry { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Physical Address Province cannot be longer than 50 characters.")]
        public string PhysicalProvince { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Physical Address Suburb cannot be longer than 50 characters.")]
        public string PhysicalAddressSuburb { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Postal Address City cannot be longer than 50 characters.")]
        public string PostalAddressCity { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Postal Address Code cannot be longer than 50 characters.")]
        public string PostalAddressCode { get; set; }
        
        [DataMember]
        [MaxLength(50, ErrorMessage = "Postal Address Line 1 cannot be longer than 50 characters.")]
        public string PostalAddressLine1 { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Postal Address Line 2 cannot be longer than 50 characters.")]
        public string PostalAddressLine2 { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Postal Address Suburb cannot be longer than 50 characters.")]
        public string PostalAddressSuburb { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Postal Address Country cannot be longer than 50 characters.")]
        public string PostalCountry { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Postal Province cannot be longer than 50 characters.")]
        public string PostalProvince { get; set; }

        [DataMember]
        public string PostalCodeWork { get; set; }

        [DataMember]
        [Required]
        public string PreferredContactDelivery { get; set; }

        [DataMember]
        public string PreferredName { get; set; }

        [DataMember]
        public string PreviousEmployerName { get; set; }

        [DataMember]
        public string PreviousEmployerPeriod { get; set; }

        [DataMember]
        public string RelationshipType { get; set; }

        [DataMember]
        public bool SACitizen { get; set; }

        [DataMember]
        [Required]
        public string SAHLSACitizenType { get; set; }

        [DataMember]
        public bool SAHLDeclaration { get; set; }

        [DataMember]
        public string SAHLHighestQualification { get; set; }

        [DataMember]
        [Required]
        public bool SAHLIsSurety { get; set; }

        [DataMember]
        [Required]
        public string Surname { get; set; }

        [DataMember]
        [Required]
        public string Title { get; set; }

        [DataMember]
        public string WorkPhone { get; set; }

        [DataMember]
        public string WorkPhoneCode { get; set; }

        [DataMember]
        public List<IncomeItem> IncomeItems { get; set; }

        [DataMember]
        public List<ExpenditureItem> ExpenditureItems { get; set; }

        [DataMember]
        public List<AssetItem> AssetItems { get; set; }

        [DataMember]
        public List<LiabilityItem> LiabilityItems { get; set; }

        [DataMember]
        public List<BankAccount> BankAccounts { get; set; }
    }
}