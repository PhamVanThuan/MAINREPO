using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Services.CommandPersistence.CommandRetryPolicy;
using SAHL.Core.Services.Metrics;
using SAHL.Core.Web.Services;
using System;
using System.Net.Http;

namespace SAHL.Core.Web.Specs.CommandHttpHandlerBaseControllerSpecs
{
    public class when_performing_failing_command_with_retry_policy : WithFakes
    {
        private static CommandHttpHandlerBaseController commandController;
        private static IServiceCommandRouter commandRouter;
        private static ICommandServiceRequestMetrics commandMetrics;
        private static IHttpCommandAuthoriser commandAuthoriser;
        private static IJsonActivator jsonActivator;
        private static IMetadataManager metadataManager;
        private static IServiceCommand serviceCommand;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ICommandSession commandSession;
        private static ICommandSessionFactory commandSessionFactory;
        private static ICommandRetryPolicy commandRetryPolicy;
        private static ICommand command;
        private const string contentString = "";
        private static HttpResponseMessage result;
        private static ICommandDataManager commandDataManager;
        private static IHttpCommandRun httpCommandRun;

        protected Establish context = () =>
        {
            serviceCommand = An<IServiceCommand>();
            jsonActivator = An<IJsonActivator>();
            commandAuthoriser = An<IHttpCommandAuthoriser>();
            metadataManager = An<IMetadataManager>();
            commandRouter = An<IServiceCommandRouter>();
            commandMetrics = An<ICommandServiceRequestMetrics>();
            logger = new FakeLogger();
            loggerSource = An<ILoggerSource>();
            commandDataManager = An<ICommandDataManager>();
            commandSession = An<CommandSession>(commandDataManager, An<ICommandRetryPolicy>());
            command = An<ICommand>();
            command.CommandJson = "";

            commandSession.CommandKey = 1;
            commandRetryPolicy = An<CommandRetryPolicyThree>();

            var authToken = new HttpCommandAuthoriser.AuthToken();

            jsonActivator.WhenToldTo(x => x.DeserializeCommand(contentString)).Return(serviceCommand);
            commandAuthoriser.WhenToldTo(x => x.AuthoriseCommand(serviceCommand)).Return(authToken);
            commandAuthoriser.WhenToldTo(x => x.AuthoriseCommand(Arg.Any<IServiceCommand>())).Throw(new Exception());
            commandRetryPolicy = Substitute.For<CommandRetryPolicyThree>();

            commandSessionFactory = An<ICommandSessionFactory>();
            commandSessionFactory.WhenToldTo(x => x.CreateNewCommandManager("")).Return(commandSession);

            commandDataManager.WhenToldTo(x => x.CreateCommand(Arg.Any<string>(), null)).Return(command);
            commandDataManager.WhenToldTo(x => x.SaveCommand(Arg.Any<ICommand>(), Arg.Any<string>(), "")).Return(1);
            httpCommandRun = An<HttpCommandRun>(commandAuthoriser, jsonActivator, metadataManager, logger, commandRouter, commandMetrics, loggerSource);
            commandController = new CommandHttpHandlerBaseController(logger, loggerSource, commandSessionFactory, httpCommandRun, An<IJsonActivator>(), An<IHostContext>());

            commandController.Request = new HttpRequestMessage();
            commandController.Request.Content = new FakeHttpContent(contentString);
        };

        private Because of = async () =>
        {
            result = await commandController.PerformHttpCommand() as HttpResponseMessage;
        };

        private It should_persist_command = () =>
        {
            commandSession.WasToldTo(x => x.PersistCommand(""));
        };

        private It should_ask_if_retry = () =>
        {
            commandRetryPolicy.WasToldTo(x => x.ShouldRetry());
        };

        private It should_mark_command_fail = () =>
        {
            commandSession.WasToldTo(x => x.FailCommand(Arg.Any<string>()));
        };
    }
}