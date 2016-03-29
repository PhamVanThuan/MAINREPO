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

namespace SAHL.Services.LifeDomain.Specs.RuleSpecs.ClientCanOnlyHaveOnePendingDisabilityClaim
{
    public class when_the_client_has_no_previous_disability_claims : WithFakes
    {
        private static AccountCanOnlyHaveOnePendingOrApprovedDisabilityClaimRule rule;
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static DisabilityClaimLifeAccountTestModel ruleModel;
        private static IEnumerable<DisabilityClaimModel> existingDisabilityClaims;
        private static ISystemMessageCollection messages;
        private static int lifeAccountKey;

        private Establish context = () =>
        {
            lifeAccountKey = 1;
            messages = SystemMessageCollection.Empty();
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            ruleModel = new DisabilityClaimLifeAccountTestModel(lifeAccountKey);
            rule = new AccountCanOnlyHaveOnePendingOrApprovedDisabilityClaimRule(lifeDomainDataManager);
            existingDisabilityClaims = Enumerable.Empty<DisabilityClaimModel>();
            lifeDomainDataManager.WhenToldTo(x => x.GetDisabilityClaimsByAccount(ruleModel.LifeAccountKey)).Return(existingDisabilityClaims);
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