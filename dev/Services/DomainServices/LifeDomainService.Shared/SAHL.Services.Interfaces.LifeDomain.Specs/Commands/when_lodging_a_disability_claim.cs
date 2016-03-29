using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using SAHL.Core.Identity;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Commands
{
    public class when_lodging_a_disability_claim : WithFakes
    {
        private static LodgeDisabilityClaimCommand command;
        private static int accountKey;
        private static int legalEntityKey;
        private static Guid disabilityClaimGuid;

        private Establish context = () =>
        {
            disabilityClaimGuid = CombGuid.Instance.Generate();
            accountKey = 2222;
            legalEntityKey = 3333;
        };

        private Because of = () =>
        {
            command = new LodgeDisabilityClaimCommand(accountKey, legalEntityKey, disabilityClaimGuid);
        };

        private It should_be_assignable_to_ILifeDomainCommand = () =>
        {
            command.ShouldBeAssignableTo(typeof(ILifeDomainCommand));
        };
    }
}