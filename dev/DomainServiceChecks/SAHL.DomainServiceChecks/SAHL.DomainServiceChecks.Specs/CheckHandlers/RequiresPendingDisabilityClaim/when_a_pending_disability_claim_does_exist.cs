using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.LifeDataManager;
using System;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresPendingDisabilityClaim
{
    public class when_a_pending_disability_claim_does_exist : WithFakes
    {
        private static ILifeDataManager lifeDataManager;
        private static RequiresPendingDisabilityClaimHandler handler;
        private static IRequiresPendingDisabilityClaim command;
        private static ISystemMessageCollection systemMessages;
        private static DisabilityClaimDataModel disabilityClaim;

        private Establish context = () =>
        {
            lifeDataManager = An<ILifeDataManager>();
            command = An<IRequiresPendingDisabilityClaim>();
            handler = new RequiresPendingDisabilityClaimHandler(lifeDataManager);
            disabilityClaim = new DisabilityClaimDataModel(1, 1, DateTime.Now.AddDays(-5), DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-20),
                "Rally Driver", (int)DisabilityType.HeadInjury, "Mental", DateTime.Now.AddYears(1), (int)DisabilityClaimStatus.Pending, DateTime.Now.AddMonths(4),
                6, DateTime.Now.AddMonths(10));
            lifeDataManager.WhenToldTo(x => x.GetDisabilityClaimByKey(command.DisabilityClaimKey)).Return(disabilityClaim);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(command);
        };

        private It should_check_for_disability_claim = () =>
        {
            lifeDataManager.WasToldTo(x => x.GetDisabilityClaimByKey(command.DisabilityClaimKey));
        };

        private It should_contain_no_error_messages = () =>
        {
            systemMessages.ErrorMessages().ShouldBeEmpty();
        };
    }
}