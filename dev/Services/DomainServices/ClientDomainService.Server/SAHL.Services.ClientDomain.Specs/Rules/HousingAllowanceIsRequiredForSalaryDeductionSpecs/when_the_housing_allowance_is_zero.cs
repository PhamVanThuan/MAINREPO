using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.HousingAllowanceIsRequiredForSalaryDeductionSpecs
{
    public class when_the_housing_allowance_is_zero : WithFakes
    {
        static HousingAllowanceIsRequiredForSalaryDeductionRule rule;
        static ISystemMessageCollection messages;
        static SalaryDeductionEmploymentModel model;
        static EmployerModel employer;
        Establish context = () =>
        {
            rule = new HousingAllowanceIsRequiredForSalaryDeductionRule();
            employer = new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            messages = SystemMessageCollection.Empty();
            model = new SalaryDeductionEmploymentModel(10000, 0, 25, employer, DateTime.MinValue, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        It should_return_a_message_indicating_the_housing_allowance_is_required = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Housing allowance is required and must be greater than R 0.");
        };
    }
}