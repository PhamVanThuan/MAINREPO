using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.EmploymentStartDateMustBeBeforeTodaySpecs
{
    public class when_the_start_date_is_today : WithFakes
    {
        static EmploymentStartDateMustBeBeforeTodayRule<SalaryDeductionEmploymentModel> rule;
        static SalaryDeductionEmploymentModel model;
        static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            rule = new EmploymentStartDateMustBeBeforeTodayRule<SalaryDeductionEmploymentModel>();
            messages = SystemMessageCollection.Empty();
            model = new SalaryDeductionEmploymentModel(10000, 0, 25, new EmployerModel(1234, "Test", "", "", "", "", EmployerBusinessType.Company, EmploymentSector.FinancialServices), 
                DateTime.Today, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain("The start date of a current employment record cannot be today.");
        };
    }
}