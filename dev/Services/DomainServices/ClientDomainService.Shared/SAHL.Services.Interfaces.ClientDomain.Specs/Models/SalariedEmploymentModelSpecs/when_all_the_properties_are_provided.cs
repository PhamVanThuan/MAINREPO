using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.SalariedEmploymentModelSpecs
{
    public class when_all_the_properties_are_provided : WithFakes
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
            salaryPaymentDay = 1;
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

        It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        It should_construct_the_model = () =>
        {
            salariedModel.ShouldNotBeNull();
        };
    }
}
