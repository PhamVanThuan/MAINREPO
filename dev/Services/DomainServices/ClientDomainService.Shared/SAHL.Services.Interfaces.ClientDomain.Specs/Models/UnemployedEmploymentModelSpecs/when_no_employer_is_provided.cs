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

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.UnemployedEmploymentModelSpecs
{
    public class when_no_employer_is_provided : WithFakes
    {
        private static UnemployedEmploymentModel salaryDeductionModel;
        private static Exception ex;
        private static double basicIncome;
        private static DateTime startDate;
        private static EmploymentStatus employmentStatus;
        private static double housingAllowance;
        private static int salaryPaymentDay;
        private static EmployerModel employer;
        private static UnemployedRemunerationType remunerationType;

        Establish context = () =>
        {
            employer = null;
            basicIncome = 25000;
            housingAllowance = 10000;
            salaryPaymentDay = 1;
            startDate = Convert.ToDateTime(null);
            remunerationType = UnemployedRemunerationType.InvestmentIncome;
            employmentStatus = EmploymentStatus.Current;
        };

        Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                salaryDeductionModel = new UnemployedEmploymentModel(basicIncome, salaryPaymentDay, employer, startDate, remunerationType, employmentStatus);
            });
        };

        It should_throw_an_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        It should_provide_a_message = () =>
        {
            ex.Message.ShouldEqual("The Employer field is required.");
        };
    }
}
