using SAHL.Core.Identity;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Capitec.Specs.Stubs
{
    public class ApplicantStubs
    {
        public Guid SalutationEnumId = CombGuid.Instance.Generate();
        public Guid SalutationEnumId_1 = CombGuid.Instance.Generate();
        public Guid ApplicantID = Guid.NewGuid();

        private ApplicantInformation _applicantInformation;

        public ApplicantInformation GetApplicantInformation
        {
            get
            {
                if (_applicantInformation == null)
                {
                    _applicantInformation = new ApplicantInformation("112233", "Bob", "Muggs", SalutationEnumId, "0724310696", "0724310696", "0724310696", "bob@muggs.com", DateTime.Parse("1979-01-10"), "Mr", true);
                }
                return _applicantInformation;
            }
        }

        private ApplicantInformation _secondApplicantInformation;

        public ApplicantInformation GetSecondApplicantInformation
        {
            get
            {
                if (_secondApplicantInformation == null)
                {
                    _secondApplicantInformation = new ApplicantInformation("332211", "Jane", "Huggs", SalutationEnumId_1, "0724310696", "0724310696", "0724310696", "Jane@Huggs.com", DateTime.Parse("1982-11-04"), "Mr", true);
                }
                return _secondApplicantInformation;
            }
        }

        private Applicant _applicant;

        public Applicant GetApplicant
        {
            get
            {
                if (_applicant == null)
                {
                    _applicant = new Applicant(GetApplicantInformation, GetApplicantResidentialAddress, GetApplicantEmploymentDetails, GetApplicantDeclarations);
                }
                return _applicant;
            }
        }

        private Applicant _secondapplicant;

        public Applicant GetSecondApplicant
        {
            get
            {
                if (_secondapplicant == null)
                {
                    _secondapplicant = new Applicant(GetSecondApplicantInformation, GetApplicantResidentialAddress, GetApplicantEmploymentDetails, GetApplicantDeclarations);
                }
                return _secondapplicant;
            }
        }

        private ApplicantResidentialAddress _applicantResidentialAddress;

        public ApplicantResidentialAddress GetApplicantResidentialAddress
        {
            get
            {
                if (_applicantResidentialAddress == null)
                {
                    _applicantResidentialAddress = new ApplicantResidentialAddress("C", "46", "Luthuli House", "90", "Smith Street", "Sandton", "Gauteng", "Johannesburg", "2000", new Guid());
                }
                return _applicantResidentialAddress;
            }
        }

        private ApplicantEmploymentDetails _applicantEmploymentDetails;

        public ApplicantEmploymentDetails GetApplicantEmploymentDetails
        {
            get
            {
                if (_applicantEmploymentDetails == null)
                {
                    if (ThreeMonthAverageCommission.HasValue)
                    {
                        _applicantEmploymentDetails = new ApplicantEmploymentDetails(EmploymentTypeEnumId, null, SalariedWithCommissionDetails, null, null, null);
                        return _applicantEmploymentDetails;
                    }

                    if (HousingAllowance.HasValue)
                    {
                        _applicantEmploymentDetails = new ApplicantEmploymentDetails(EmploymentTypeEnumId, null, null, SalariedWithHousingAllowanceDetails, null, null);
                        return _applicantEmploymentDetails;
                    }

                    if (SelfEmployedGrossMonthlyIncome.HasValue)
                    {
                        _applicantEmploymentDetails = new ApplicantEmploymentDetails(EmploymentTypeEnumId, null, null, null, SelfEmployedDetails, null);
                        return _applicantEmploymentDetails;
                    }

                    _applicantEmploymentDetails = new ApplicantEmploymentDetails(EmploymentTypeEnumId, SalariedDetails, null, null, null, null);
                }
                return _applicantEmploymentDetails;
            }
        }

        private ApplicantDeclarations _applicantDeclarations;

        public ApplicantDeclarations GetApplicantDeclarations
        {
            get
            {
                if (_applicantDeclarations == null)
                {
                    _applicantDeclarations = new ApplicantDeclarations(IncomeContributor, AllowCreditBureauCheck, HasCapitecTransactionAccount, MarriedInCommunityOfProperty);
                }
                return _applicantDeclarations;
            }
        }

        private SalariedDetails _salariedDetails;

        public SalariedDetails SalariedDetails
        {
            get
            {
                if (_salariedDetails == null && GrossMonthlyIncome.HasValue)
                {
                    _salariedDetails = new SalariedDetails(GrossMonthlyIncome.Value);
                }
                return _salariedDetails;
            }
        }

        private SalariedWithCommissionDetails _salariedWithCommissionDetails;
        public SalariedWithCommissionDetails SalariedWithCommissionDetails
        {
            get
            {
                if (_salariedWithCommissionDetails == null && (GrossMonthlyIncome.HasValue && ThreeMonthAverageCommission.HasValue))
                {
                    _salariedWithCommissionDetails = new SalariedWithCommissionDetails(GrossMonthlyIncome.Value, ThreeMonthAverageCommission.Value);
                }
                return _salariedWithCommissionDetails;
            }
        }

        private SalariedWithHousingAllowanceDetails _salariedWithHousingAllowanceDetails;
   
        public SalariedWithHousingAllowanceDetails SalariedWithHousingAllowanceDetails
        {
            get
            {
                if (_salariedWithHousingAllowanceDetails == null && (GrossMonthlyIncome.HasValue && HousingAllowance.HasValue))
                {
                    _salariedWithHousingAllowanceDetails = new SalariedWithHousingAllowanceDetails(GrossMonthlyIncome.Value, HousingAllowance.Value);
                }
                return _salariedWithHousingAllowanceDetails;
            }
        }

        private SelfEmployedDetails _selfEmployedDetails;
        public SelfEmployedDetails SelfEmployedDetails
        {
            get
            {
                if (_selfEmployedDetails == null && SelfEmployedGrossMonthlyIncome.HasValue)
                {
                    _selfEmployedDetails = new SelfEmployedDetails(SelfEmployedGrossMonthlyIncome.Value);
                }
                return _selfEmployedDetails;
            }
        }

        public Guid IncomeContributor { get; set; }

        public Guid AllowCreditBureauCheck { get; set; }

        public DateTime DeclarationsDate { get; set; }

        public Guid HasCapitecTransactionAccount { get; set; }

        public Guid MarriedInCommunityOfProperty { get; set; }

        public decimal? GrossMonthlyIncome { get; set; }
        public decimal? ThreeMonthAverageCommission { get; set; }

        public decimal? HousingAllowance { get; set; }
        public decimal? SelfEmployedGrossMonthlyIncome { get; set; }
        public Guid EmploymentTypeEnumId { get; set; }

        public int CalculateAge(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }
    }
}