using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Commands
{
    public class when_repudiating_a_disability_claim : WithFakes
    {
        private static RepudiateDisabilityClaimCommand command;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            disabilityClaimKey = 1111;
        };

        private Because of = () =>
        {
            command = new RepudiateDisabilityClaimCommand(disabilityClaimKey, Param.IsAny<IEnumerable<int>>());
        };

        private It should_be_assignable_to_ILifeDomainCommand = () =>
        {
            command.ShouldBeAssignableTo(typeof(ILifeDomainCommand));
        };
    }
}