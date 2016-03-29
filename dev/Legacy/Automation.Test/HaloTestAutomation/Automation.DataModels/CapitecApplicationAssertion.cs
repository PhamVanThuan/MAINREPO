using Common.Enums;
using SAHL.Services.Capitec.Models.Shared;
using System;
namespace Automation.DataModels
{
    public sealed class CapitecApplicationAssertion
    {
        public OfferStatusEnum ExpectedOfferStatusKey;
        public int ExpectedLAA { get; set; }
        public string ExpectedIdNumber { get; set; }
        public int ExpectedLegalEntityKey { get; set; }
        public MortgageLoanPurposeEnum ExpectedLoanPurpose { get; set; }
        public int ExpectedTerm { get; set; }
        public int ExpectedHouseholdIncome { get; set; }
        public EmploymentTypeEnum ExpectedEmploymentTypeKey { get; set; }
        public int ExpectedApplicationKey { get; set; }
        public bool HasITC { get; set; }
        public ConsultantDetails ExpectedConsultantDetails { get; set; }
        public string ExpectedStreetNumber { get; set; }
        public string ExpectedStreetName { get; set; }
        public string ExpectedProvince { get; set; }
        public string ExpectedSuburb { get; set; }
        public DateTime ExpectedAddressChangeDate { get; set; }
        public string ExpectedAddressUserID { get; set; }
        public int ExpectedAddressKey { get; set; }
        public int ExpectedEmploymentKey { get; set; }
        public int ExpectedLegalEntityAddressKey { get; set; }
        public bool IsMainContact { get; set; }
        public int ExpectedConsultantLegalEntityKey { get; set; }
        public string ExpectedAssignedConsultant { get; set; }
        public string ExpectedConsultant { get; set; }
        public string ExpectedTeleConsultant { get; set; }
        public Automation.DataModels.LegalEntity ExpectedLegalEntity { get; set; }
        public string ExpectedApplicationReasonType { get; set; }
        public string ExpectedApplicationReasonDescription { get; set; }
        public bool HasApplicationReasons { get; set; }
        public bool IsITCLinkedToLegalEntity { get; set; }
        public AuditLegalEntity ExpectedAuditLegalEntity { get; set; }
        public DirtyLegalEntityDetails DirtyLegalEntityDetails { get; set; }
        public CapitecApplicationAssertion Copy()
        {
            return new CapitecApplicationAssertion()
            {
                ExpectedAuditLegalEntity = new AuditLegalEntity{
                    FirstNames = this.ExpectedAuditLegalEntity.FirstNames,
                    Surname = this.ExpectedAuditLegalEntity.Surname,
                    WorkPhoneCode= this.ExpectedAuditLegalEntity.WorkPhoneCode,
                    WorkPhoneNumber= this.ExpectedAuditLegalEntity.WorkPhoneNumber,
                    HomePhoneCode = this.ExpectedAuditLegalEntity.HomePhoneCode,
                    HomePhoneNumber = this.ExpectedAuditLegalEntity.HomePhoneNumber
                },
                ExpectedLAA = this.ExpectedLAA,
                ExpectedIdNumber = this.ExpectedIdNumber,
                ExpectedLegalEntityKey = this.ExpectedLegalEntityKey,
                ExpectedLoanPurpose = this.ExpectedLoanPurpose,
                ExpectedTerm = this.ExpectedTerm,
                ExpectedHouseholdIncome = this.ExpectedHouseholdIncome,
                ExpectedEmploymentTypeKey = this.ExpectedEmploymentTypeKey,
                ExpectedApplicationKey = this.ExpectedApplicationKey,
                HasITC = this.HasITC,
                ExpectedConsultantDetails = new ConsultantDetails(this.ExpectedConsultantDetails.Name, this.ExpectedConsultantDetails.Branch),
                ExpectedStreetNumber = this.ExpectedStreetNumber,
                ExpectedStreetName = this.ExpectedStreetName,
                ExpectedProvince = this.ExpectedProvince,
                ExpectedSuburb = this.ExpectedSuburb,
                ExpectedAddressKey = this.ExpectedAddressKey,
                ExpectedEmploymentKey = this.ExpectedEmploymentKey,
                ExpectedLegalEntityAddressKey = this.ExpectedLegalEntityAddressKey,
                IsMainContact = this.IsMainContact,
                ExpectedConsultantLegalEntityKey = this.ExpectedConsultantLegalEntityKey,
                ExpectedAssignedConsultant = this.ExpectedAssignedConsultant,
                ExpectedConsultant = this.ExpectedConsultant,
                ExpectedTeleConsultant = this.ExpectedTeleConsultant,
                ExpectedAddressChangeDate = this.ExpectedAddressChangeDate,
                ExpectedAddressUserID = this.ExpectedAddressUserID
            };
        }

        public bool CheckDirtyLegalEntityNotUpdated { get; set; }
    }
    public class DirtyLegalEntityDetails
    {
        public int LegalEntityKey { get; set; }
        public string IDNumber { get; set; }
        public string Surname { get; set; }
        public string FirstNames { get; set; }
        public SalutationTypeEnum Salutation { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
