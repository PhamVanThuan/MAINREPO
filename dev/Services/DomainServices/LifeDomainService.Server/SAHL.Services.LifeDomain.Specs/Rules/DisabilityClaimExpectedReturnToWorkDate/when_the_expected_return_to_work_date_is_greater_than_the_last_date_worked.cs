using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.Specs.RuleSpecs.DisabilityClaimExpectedReturnToWorkDate
{
    public class when_the_expected_return_to_work_date_is_greater_than_the_last_date_worked : WithFakes
    {
        private static ExpectedReturnDateMustBeAfterLastDateWorkedRule rule;
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static DisabilityClaimRuleTestModel ruleModel;
        private static ISystemMessageCollection messages;
        private static DateTime expectedReturnToWorkDate;
        private static DateTime lastDateWorked;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            rule = new ExpectedReturnDateMustBeAfterLastDateWorkedRule(lifeDomainDataManager);
            lastDateWorked = DateTime.Now.AddMonths(-6);
            expectedReturnToWorkDate = lastDateWorked.AddMonths(5);

            ruleModel = new DisabilityClaimRuleTestModel(expectedReturnToWorkDate, lastDateWorked);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}