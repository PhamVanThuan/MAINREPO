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
    public class when_the_client_has_an_existing_pending_disability_claim : WithFakes
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
            rule = new AccountCanOnlyHaveOnePendingOrApprovedDisabilityClaimRule(lifeDomainDataManager);
            ruleModel = new DisabilityClaimLifeAccountTestModel(lifeAccountKey);
            existingDisabilityClaims = new DisabilityClaimModel[] { new DisabilityClaimModel(1, 0, 0
                                                                        , DateTime.Now, null, null, null, null, null, null
                                                                        , (int)DisabilityClaimStatus.Pending, null, null, null) 
                                                                   };

            lifeDomainDataManager.WhenToldTo(x => x.GetDisabilityClaimsByAccount(ruleModel.LifeAccountKey)).Return(existingDisabilityClaims);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().Where(x => x.Message == "The client already has a pending or approved disability claim.").First().ShouldNotBeNull();
        };
    }
}