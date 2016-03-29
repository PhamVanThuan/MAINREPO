using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Commands
{
    public class when_deleting_a_disability_claim : WithFakes
    {
        public static CompensateLodgeDisabilityClaimCommand command;
        public static int disabilityClaimKey;

        private Establish context = () =>
        {
            disabilityClaimKey = 111;
        };

        private Because of = () =>
        {
            command = new CompensateLodgeDisabilityClaimCommand(disabilityClaimKey);
        };

        private It should_be_assignable_to_ILifeDomainCommand = () =>
        {
            command.ShouldBeAssignableTo(typeof(ILifeDomainCommand));
        };
    }
}