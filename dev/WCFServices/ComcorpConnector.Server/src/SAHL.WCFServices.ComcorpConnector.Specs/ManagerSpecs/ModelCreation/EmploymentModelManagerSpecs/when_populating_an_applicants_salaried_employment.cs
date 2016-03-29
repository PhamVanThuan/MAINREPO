using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.EmploymentModelManagerSpecs
{
    public class when_populating_an_applicants_salaried_employment : WithCoreFakes
    {
        private static EmploymentModelManager modelManager;
        private static ValidationUtils _validationUtils;
        private static SalariedEmploymentModel employmentModel;
        private static Applicant applicant;

        private Establish context = () =>
        {
            _validationUtils = new ValidationUtils();
            modelManager = new EmploymentModelManager(_validationUtils);
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant, MaritalStatus.Single, false,
                false, false, false, true);
            applicant.EmployerName = "Google";
            applicant.EmployerEmail = "contact@google.com";
            applicant.DateJoinedEmployer = DateTime.Now.AddYears(-3);
            applicant.CurrentEmploymentType = "Salaried";
            applicant.EmploymentSector = "IT and Electronics";
            applicant.EmployerBusinessType = "Company";
            applicant.DateSalaryPaid = 29;
            applicant.EmployerGrossMonthlySalary = 15000;
        };

        private Because of = () =>
        {
            employmentModel = modelManager.PopulateEmployment(applicant).First() as SalariedEmploymentModel;
        };

        private It should_map_the_employment_type = () =>
        {
            employmentModel.EmploymentType.ShouldEqual(EmploymentType.Salaried);
        };

        private It should_map_the_remuneration_type = () =>
        {
            employmentModel.RemunerationType.ShouldEqual(RemunerationType.Salaried);
        };

        private It should_map_the_salariedremuneration_type = () =>
        {
            employmentModel.SalariedRemunerationType.ShouldEqual(SalariedRemunerationType.Salaried);
        };

        private It should_map_the_basic_income_value_to_the_EmployerGrossMonthlySalary_field = () =>
        {
            employmentModel.BasicIncome.ShouldEqual(Convert.ToDouble(applicant.EmployerGrossMonthlySalary));
        };

        private It should_map_the_employment_start_date_to_the_DateJoinedEmployer_field = () =>
        {
            employmentModel.StartDate.ShouldEqual(applicant.DateJoinedEmployer);
        };

        private It should_set_the_employment_status_to_current = () =>
        {
            employmentModel.EmploymentStatus.ShouldEqual(EmploymentStatus.Current);
        };

        private It should_add_the_employer_model_to_the_employment = () =>
        {
            employmentModel.Employer.EmployerName.ShouldEqual(applicant.EmployerName);
        };

        private It should_set_the_employer_business_type = () =>
        {
            employmentModel.Employer.EmployerBusinessType.ShouldEqual(EmployerBusinessType.Company);
        };

        private It should_set_the_employer_sector = () =>
        {
            employmentModel.Employer.EmploymentSector.ShouldEqual(EmploymentSector.ITandElectronics);
        };

        private It should_map_the_salary_payment_day_to_the_DateSalaryPaid_field = () =>
        {
            employmentModel.SalaryPaymentDay.ShouldEqual(applicant.DateSalaryPaid);
        };

        private It should_populate_the_employee_medical_aid_deduction = () =>
        {
            employmentModel.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.MedicalAid).FirstOrDefault().ShouldNotBeNull();
        };

        private It should_populate_the_employee_paye_deduction = () =>
        {
            employmentModel.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.PayAsYouEarnTax).FirstOrDefault().ShouldNotBeNull();
        };

        private It should_populate_the_PensionOrProvidendFund_deduction = () =>
        {
            employmentModel.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.PensionOrProvidendFund).FirstOrDefault().ShouldNotBeNull();
        };

        private It should_populate_the_employee_UnemploymentInsurance_deduction = () =>
        {
            employmentModel.EmployeeDeductions.Where(x => x.Type == EmployeeDeductionTypeEnum.UnemploymentInsurance).FirstOrDefault().ShouldNotBeNull();
        };
    }
}