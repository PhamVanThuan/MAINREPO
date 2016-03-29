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

namespace SAHL.Services.LifeDomain.Specs.RuleSpecs.DisabilityClaimAuthorisedInstalmentsMaximum
{
    public class when_the_authorised_instalments_is_within_the_maximum_allowed : WithFakes
    {
        private static DisabilityClaimAuthorisedInstalmentsMaximumRule rule;
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static ISystemMessageCollection messages;
        private static AuthorisedInstalmentsMaxTestModel ruleModel;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            rule = new DisabilityClaimAuthorisedInstalmentsMaximumRule(lifeDomainDataManager);
            ruleModel = new AuthorisedInstalmentsMaxTestModel(12);
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