using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.BasicIncomeRequiredSpecs
{
    public class when_basic_income_is_zero_for_salary_deduction : WithFakes
    {
        private static BasicIncomeIsRequiredRule<SalaryDeductionEmploymentModel> rule;
        private static SalaryDeductionEmploymentModel model;
        private static ISystemMessageCollection messages;
        private static EmployerModel employer;

        private Establish context = () =>
        {
            rule = new BasicIncomeIsRequiredRule<SalaryDeductionEmploymentModel>();
            messages = SystemMessageCollection.Empty();
            employer = new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            model = new SalaryDeductionEmploymentModel(0, 10000, 25, employer, DateTime.MinValue, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_add_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Basic Income must be greater than zero");
        };
    }
}