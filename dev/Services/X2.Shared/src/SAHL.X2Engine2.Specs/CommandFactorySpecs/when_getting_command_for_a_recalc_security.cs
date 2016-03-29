using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandFactorySpecs
{
    public class when_getting_command_for_a_recalc_security : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker;
        private static IX2Request request;
        private static IEnumerable<IServiceCommand> commands;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            automocker = new NSubstituteAutoMocker<CommandFactory>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2RequestForSecurityRecalc(Guid.NewGuid(), 123, serviceRequestMetadata);
        };

        Because of = () =>
            {
                commands = automocker.ClassUnderTest.GetSecurityRecalc(request);
            };

        It should_return_a_single_command = () =>
            {
                commands.Count().ShouldEqual(1);
            };

        It should_return_a_RebuildInstanceCommand = () =>
            {
                commands.First().ShouldBe(typeof(RebuildInstanceCommand));
            };
    }
}