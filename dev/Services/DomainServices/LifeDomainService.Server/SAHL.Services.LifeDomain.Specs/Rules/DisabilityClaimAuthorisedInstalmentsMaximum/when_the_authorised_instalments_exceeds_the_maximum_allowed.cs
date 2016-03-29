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
    public class when_the_authorised_instalments_exceeds_the_maximum_allowed : WithFakes
    {
        private static DisabilityClaimAuthorisedInstalmentsMaximumRule rule;
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static AuthorisedInstalmentsMaxTestModel ruleModel;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            rule = new DisabilityClaimAuthorisedInstalmentsMaximumRule(lifeDomainDataManager);
            ruleModel = new AuthorisedInstalmentsMaxTestModel(100);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().Where(x => x.Message == "No. of Authorised Instalments cannot be greater than 99.").First().ShouldNotBeNull();
        };
    }
}