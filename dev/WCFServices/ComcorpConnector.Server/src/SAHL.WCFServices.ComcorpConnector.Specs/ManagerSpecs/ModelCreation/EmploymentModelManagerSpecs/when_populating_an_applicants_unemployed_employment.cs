using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.EmploymentModelManagerSpecs
{
    public class when_populating_an_applicants_unemployed_employment_with_valid_remuneration : WithCoreFakes
    {
        private static EmploymentModelManager modelManager;
        private static ValidationUtils _validationUtils;
        private static UnemployedEmploymentModel employmentModel;
        private static Applicant applicant;
        private static string remuneration;

        private Establish context = () =>
        {
            _validationUtils = new ValidationUtils();
            modelManager = new EmploymentModelManager(_validationUtils);
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant
                , MaritalStatus.Single
                , false
                , false
                , false
                , false
                , true);
            applicant.EmployerName = "Google";
            applicant.EmployerEmail = "contact@google.com";
            applicant.DateJoinedEmployer = DateTime.Now.AddYears(-3);
            applicant.CurrentEmploymentType = "UnEmployed";
            applicant.EmploymentSector = "IT and Electronics";
            applicant.EmployerBusinessType = "Company";
            applicant.DateSalaryPaid = 29;
            applicant.EmployerGrossMonthlySalary = 15000;

            var validRemunerations = Enum.GetValues(typeof(UnemployedRemunerationType))
                .Cast<UnemployedRemunerationType>()
                .Select(x => x.ToString());
            Random rand = new Random();

            remuneration = validRemunerations.ElementAt(rand.Next(0, validRemunerations.Count() - 1));
            applicant.EmployerRemunerationType = remuneration;
        };

        private Because of = () =>
        {
            employmentModel = modelManager.PopulateEmployment(applicant).First() as UnemployedEmploymentModel;
        };

        private It should_map_the_employment_type = () =>
        {
            employmentModel.EmploymentType.ShouldEqual(EmploymentType.Unemployed);
        };

        private It should_map_the_remuneration_type = () =>
        {
            employmentModel.RemunerationType.ToString().ShouldEqual(remuneration);
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

        private It should_map_the_income_payment_day_to_the_DateSalaryPaid_field = () =>
        {
            employmentModel.SalaryPaymentDay.ShouldEqual(applicant.DateSalaryPaid);
        };
    }
}