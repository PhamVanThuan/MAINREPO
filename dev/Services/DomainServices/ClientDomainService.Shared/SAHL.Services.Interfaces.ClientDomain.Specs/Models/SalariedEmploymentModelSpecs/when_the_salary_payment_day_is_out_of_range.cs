using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.SalariedEmploymentModelSpecs
{
    public class when_the_salary_payment_day_is_out_of_range : WithFakes
    {
        private static SalariedEmploymentModel salariedModel;
        private static Exception ex;
        private static double basicIncome;
        private static DateTime startDate;
        private static EmploymentStatus employmentStatus;
        private static int salaryPaymentDay;
        private static EmployerModel employer;
        private static SalariedRemunerationType remunerationType;
        private static IEnumerable<EmployeeDeductionModel> employeeDeductions;

        Establish context = () =>
        {
            employer = new EmployerModel(null, "ÄBSA", "031", "7657192", "Bob Smith", "bob@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            basicIncome = 25000;
            salaryPaymentDay = -1;
            startDate = DateTime.Now.AddDays(-4);
            remunerationType = SalariedRemunerationType.Salaried;
            employmentStatus = EmploymentStatus.Current;
            employeeDeductions = Enumerable.Empty<EmployeeDeductionModel>();
        };

        Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                salariedModel = new SalariedEmploymentModel(basicIncome, salaryPaymentDay, employer, startDate, remunerationType, employmentStatus, employeeDeductions);
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