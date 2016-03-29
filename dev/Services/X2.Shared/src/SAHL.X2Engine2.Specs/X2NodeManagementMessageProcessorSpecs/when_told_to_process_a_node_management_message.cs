using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.X2NodeManagementMessageProcessorSpecs
{
    public class when_told_to_process_a_node_management_message : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeManagementMessageProcessor> autoMocker;
        private static IX2NodeManagementMessage message;
        private static IServiceCommand command;
        private static IX2ServiceCommandRouter commandRouter;

        private Establish context = () =>
        {
            message = An<IX2NodeManagementMessage>();

            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2NodeManagementMessageProcessor>();
            commandRouter = An<IX2ServiceCommandRouter>();
            var commands = new List<IServiceCommand>();
            command = new RefreshCacheCommand("DomainService");
            commandRouter.WhenToldTo(x => x.HandleCommand(Arg.Any<RefreshCacheCommand>(), null)).Return(SAHL.Core.SystemMessages.SystemMessageCollection.Empty());
            commands.Add(command);
            autoMocker.Get<IManagementCommandFactory>().WhenToldTo(x => x.CreateCommands(message)).Return(commands);
            autoMocker.Get<IIocContainer>().WhenToldTo(x => x.GetInstance<IX2ServiceCommandRouter>()).Return(commandRouter);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.ProcessMessage(message);
        };

        private It should_get_commands = () =>
        {
            autoMocker.Get<IManagementCommandFactory>().WasToldTo(x => x.CreateCommands(message));
        };

        private It should_handle_the_commands = () =>
        {
            commandRouter.WasToldTo(x => x.HandleCommand(Arg.Any<RefreshCacheCommand>(), null));
        };
    }
}