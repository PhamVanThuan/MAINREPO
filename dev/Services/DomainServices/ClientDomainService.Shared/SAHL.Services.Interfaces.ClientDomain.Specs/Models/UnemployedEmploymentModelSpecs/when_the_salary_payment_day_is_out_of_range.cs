using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.UnemployedEmploymentModelSpecs
{
    public class when_the_salary_payment_day_is_out_of_range : WithFakes
    {
        private static UnemployedEmploymentModel model;
        private static Exception ex;
        private static double basicIncome;
        private static DateTime startDate;
        private static EmploymentStatus employmentStatus;
        private static int salaryPaymentDay;
        private static EmployerModel employer;
        private static UnemployedRemunerationType remunerationType;

        Establish context = () =>
        {
            employer = new EmployerModel(null, "Investments", "031", "7657192", "Bob Smith", "bob@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            basicIncome = 25000;
            salaryPaymentDay = -1;
            startDate = DateTime.Now.AddDays(-4);
            remunerationType = UnemployedRemunerationType.InvestmentIncome;
            employmentStatus = EmploymentStatus.Current;
        };

        Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new UnemployedEmploymentModel(basicIncome, salaryPaymentDay, employer, startDate, remunerationType, employmentStatus);
            });
        };

        It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        It should_provide_a_message = () =>
        {
            ex.Message.ShouldEqual("The field SalaryPaymentDay must be between 0 and 31.");
        };
    }
}