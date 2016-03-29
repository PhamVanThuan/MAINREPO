using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.HousingAllowanceIsRequiredForSalaryDeductionSpecs
{
    public class when_the_housing_allowance_is_less_than_zero : WithFakes
    {
        private static HousingAllowanceIsRequiredForSalaryDeductionRule rule;
        private static ISystemMessageCollection messages;
        private static SalaryDeductionEmploymentModel model;
        private static EmployerModel employer;

        private Establish context = () =>
        {
            rule = new HousingAllowanceIsRequiredForSalaryDeductionRule();
            employer = new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            messages = SystemMessageCollection.Empty();
            model = new SalaryDeductionEmploymentModel(10000, -1, 25, employer, DateTime.MinValue, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_a_message_indicating_the_housing_allowance_is_required = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Housing allowance is required and must be greater than R 0.");
        };
    }
}