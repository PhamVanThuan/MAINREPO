using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.X2ServiceCommandRouterSpecs
{
    public class when_handling_command: WithFakes
    {
        private static IServiceCommandHandlerProvider serviceCommandHandlerProvider;
        private static IServiceRequestMetadata metadata;
        private static IServiceCommandHandler<IRuleCommand> commandHandler;
        private static X2ServiceCommandRouter serviceCommandRouter;
        private static IRuleCommand command;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            serviceCommandHandlerProvider = An<IServiceCommandHandlerProvider>();
            serviceCommandRouter = new X2ServiceCommandRouter(serviceCommandHandlerProvider);
            command = An<RuleCommand>();
            commandHandler = An<IServiceCommandHandler<IRuleCommand>>();
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            messages = SystemMessageCollection.Empty();
            serviceCommandHandlerProvider.WhenToldTo(x => x.GetCommandHandler<IRuleCommand>()).Return(commandHandler);
        };

        private Because of = () =>
        {
            messages = serviceCommandRouter.HandleCommand(command, metadata);
        };

        private It should_get_command_handler = () =>
        {
            serviceCommandHandlerProvider.WasToldTo(x => x.GetCommandHandler<IRuleCommand>());
        };
    }
}
