using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandFactorySpecs
{
    public class when_getting_commands_for_a_system_request_group_and_the_request_is_not_a_systemrequestgroup : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker;
        private static IX2Request request;
        private static System.Exception exception;
        private static IEnumerable<IServiceCommand> commands;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            automocker = new NSubstituteAutoMocker<CommandFactory>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2CreateInstanceRequest(Guid.NewGuid(), "Activity", "Process", "Workflow", serviceRequestMetadata, false);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                commands = automocker.ClassUnderTest.GetSystemRequestGroup(request);
            });
        };

        It should_throw_an_exception = () =>
        {
            exception.Message.ShouldEqual(string.Format(@"The Request Message is not a SystemRequestGroup message: CorrelationID: {0}",
                request.CorrelationId));
        };

        It should_return_no_commands_to_execute = () =>
        {
            commands.ShouldBeNull();
        };
    }
}