using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Services.Metrics;
using SAHL.Core.Web.Services;
using System;
using System.Net.Http;

namespace SAHL.Core.Web.Specs.CommandHttpHandlerBaseControllerSpecs
{
    [Subject("SAHL.Core.Web.Services.CommandHttpHandlerBaseController.PerformHttpCommand")]
    public class when_auth_token_is_not_authenticated : WithFakes
    {
        private static CommandHttpHandlerBaseController commandController;
        private static IServiceCommandRouter commandRouter;
        private static ICommandServiceRequestMetrics commandMetrics;
        private static IHttpCommandAuthoriser commandAuthoriser;
        private static IJsonActivator jsonActivator;
        private static IMetadataManager metadataManager;
        private static ICommandSessionFactory commandSessionFactory;
        private static ICommand command;
        private static IServiceCommand serviceCommand;
        private static HttpCommandRun httpCommandRun;
        private static ICommandRetryPolicy commandRetryPolicy;
        private static ICommandDataManager commandDataManager;
        private const string contentString = "";
        private static HttpResponseMessage result;
        private static readonly IServiceRequestMetadata metadata = null;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static IHostedService hostedService;
        
        private Establish context = () =>
        {
            serviceCommand = An<IServiceCommand>();
            jsonActivator = An<IJsonActivator>();
            commandAuthoriser = An<IHttpCommandAuthoriser>();
            metadataManager = An<IMetadataManager>();
            commandRouter = An<IServiceCommandRouter>();
            commandMetrics = An<ICommandServiceRequestMetrics>();
            logger = new FakeLogger();
            loggerSource = An<ILoggerSource>();
            commandRetryPolicy = An<ICommandRetryPolicy>();
            command = An<ICommand>();
            hostedService = An<IHostedService>();

            var authToken = new HttpCommandAuthoriser.AuthToken();
            authToken.Authenticated = false;

            jsonActivator.WhenToldTo(x => x.DeserializeCommand(contentString)).Return(serviceCommand);
            commandAuthoriser.WhenToldTo(x => x.AuthoriseCommand(serviceCommand)).Return(authToken);
            commandRetryPolicy.WhenToldTo(x => x.ShouldRetry()).Return(false);
            commandDataManager = An<ICommandDataManager>();
            commandDataManager.WhenToldTo(x => x.CreateCommand(Arg.Any<string>(), Arg.Any<DateTime?>())).Return(command);
            commandSessionFactory = An<CommandSessionFactory>(commandDataManager, An<ICommandRetryPolicy>(), hostedService);
            httpCommandRun = An<HttpCommandRun>(commandAuthoriser, jsonActivator, metadataManager, logger, commandRouter, commandMetrics, loggerSource);
            commandController = new CommandHttpHandlerBaseController(logger
                , loggerSource
                , commandSessionFactory
                , httpCommandRun
                , An<IJsonActivator>()
                , An<IHostContext>());
            commandController.Request = new HttpRequestMessage();
            commandController.Request.Content = new FakeHttpContent(contentString);
        };

        private Because of = async () =>
        {
            result = await commandController.PerformHttpCommand() as HttpResponseMessage;
        };

        private It should_deserialize_the_command = () =>
        {
            jsonActivator.WasToldTo(x => x.DeserializeCommand(Param<string>.IsAnything));
        };

        private It should_check_for_authorization_of_command = () =>
        {
            commandAuthoriser.WasToldTo(x => x.AuthoriseCommand(serviceCommand));
        };

        private It should_not_get_meta_data = () =>
        {
            metadataManager.WasNotToldTo(x => x.GetMetaData());
        };

        private It should_not_increment_metric_for_command = () =>
        {
            commandMetrics.WasNotToldTo(x => x.IncrementRequestForCommand<IServiceCommand>("HandleHttpCommand"));
        };

        private It should_not_tell_service_command_router_to_handle_command = () =>
        {
            commandRouter.WasNotToldTo(x => x.HandleCommand(Param<object>.IsAnything, metadata));
        };

        private It should_not_update_request_latency_metric = () =>
        {
            commandMetrics.WasNotToldTo(x => x.UpdateRequestLatencyForCommand<IServiceCommand>("HandleHttpCommand", Param<long>.IsAnything));
        };

        private It should_set_success_status_to_false_on_result = () =>
        {
            result.IsSuccessStatusCode.ShouldEqual(false);
        };
    }
}