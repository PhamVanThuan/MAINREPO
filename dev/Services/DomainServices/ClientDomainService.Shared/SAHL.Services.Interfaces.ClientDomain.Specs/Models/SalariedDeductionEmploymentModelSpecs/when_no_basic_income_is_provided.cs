using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.SalariedDeductionEmploymentModelSpecs
{
    public class when_no_basic_income_is_provided : WithFakes
    {
        private static SalaryDeductionEmploymentModel salaryDeductionModel;
        private static Exception ex;
        private static double basicIncome;
        private static DateTime startDate;
        private static EmploymentStatus employmentStatus;
        private static double housingAllowance;
        private static int salaryPaymentDay;
        private static EmployerModel employer;
        private static SalaryDeductionRemunerationType remunerationType;
        private static IEnumerable<EmployeeDeductionModel> employeeDeductions;

        Establish context = () =>
            {
                employer = new EmployerModel(null, "ÄBSA", "031", "7657192", "Bob Smith", "bob@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
                basicIncome = -1;
                housingAllowance = 10000;
                salaryPaymentDay = 1;
                startDate = Convert.ToDateTime(null);
                remunerationType = SalaryDeductionRemunerationType.Salaried;
                employmentStatus = EmploymentStatus.Current;
                employeeDeductions = Enumerable.Empty<EmployeeDeductionModel>();
            };

        Because of = () =>
            {
                ex = Catch.Exception(() =>
                {
                    salaryDeductionModel = new SalaryDeductionEmploymentModel(basicIncome, housingAllowance, salaryPaymentDay, employer, startDate, remunerationType, employmentStatus,
                        employeeDeductions);
                });
            };

        It should_throw_an_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        It should_provide_a_message = () =>
        {
            ex.Message.ShouldEqual("Basic Income must be provided.");
        };
    }
}