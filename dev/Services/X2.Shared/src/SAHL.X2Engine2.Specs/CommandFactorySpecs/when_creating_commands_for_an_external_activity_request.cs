using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Specs.CommandFactorySpecs
{
    public class when_creating_commands_for_an_external_activity_request : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker = new NSubstituteAutoMocker<CommandFactory>();
        private static IEnumerable<IServiceCommand> commands;
        private static X2ExternalActivityRequest request;
        private static int externalActivityID = 1034;
        private static int workflowID = 12;
        private static int activatingInstanceID = 99;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
         {
             serviceRequestMetadata = new ServiceRequestMetadata();
             serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
             request = new X2ExternalActivityRequest(Guid.NewGuid(), 0, 1, externalActivityID, activatingInstanceID, workflowID, serviceRequestMetadata, new Dictionary<string, string>());
         };

        private Because of = () =>
         {
             commands = automocker.ClassUnderTest.CreateCommands(request);
         };

        private It should_return_an_external_activity_command_handler = () =>
         {
             commands.First().ShouldBeOfExactType(typeof(ExternalActivityCommand));
         };

        private It should_return_the_delete_activity_external_activity_command_handler = () =>
         {
             commands.Last().ShouldBeOfExactType(typeof(DeleteActiveExternalActivityCommand));
         };
    }
}