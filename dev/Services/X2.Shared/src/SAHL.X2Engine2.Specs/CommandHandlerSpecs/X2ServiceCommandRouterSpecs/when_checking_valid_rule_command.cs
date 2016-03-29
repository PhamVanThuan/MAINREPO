using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using System.Collections.Generic;
using SAHL.X2Engine2.Specs.CommandHandlerSpecs.X2ServiceCommandRouterSpecs.Mocks;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.X2ServiceCommandRouterSpecs
{
    public class when_checking_valid_rule_command : WithFakes
    {
        private static IServiceCommandHandlerProvider serviceCommandHandlerProvider;
        private static X2ServiceCommandRouter commandRouter;
        private static IServiceCommandHandler<IRuleCommand> commandHandler;
        private static IServiceRequestMetadata metadata;
        private static IRuleCommand command;
        private static bool result;

        private Establish context = () =>
        {
            serviceCommandHandlerProvider = An<IServiceCommandHandlerProvider>();
            commandRouter = new X2ServiceCommandRouter(serviceCommandHandlerProvider);
            command  = new MockRuleCommand();

            serviceCommandHandlerProvider = Substitute.For<IServiceCommandHandlerProvider>();
            commandHandler = Substitute.For<IServiceCommandHandler<IRuleCommand>>();
            commandHandler.HandleCommand(Arg.Any<IRuleCommand>(), Arg.Any<IServiceRequestMetadata>()).Returns(info =>
                {
                    ((MockRuleCommand) command).Result = true;
                    return SystemMessageCollection.Empty();
                });
            serviceCommandHandlerProvider.GetCommandHandler<IRuleCommand>().Returns(commandHandler);
            commandRouter = new X2ServiceCommandRouter(serviceCommandHandlerProvider);
        };

        private Because of = () =>
        {
            result = commandRouter.CheckRuleCommand(command, metadata);
        };

        private It should_handle_command_handler = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand(command, metadata));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
