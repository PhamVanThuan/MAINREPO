﻿using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Services.Metrics;
using SAHL.Core.Web.Services;
using System.Collections.Generic;
using System.Net.Http;

namespace SAHL.Core.Web.Specs.CommandHttpHandlerBaseControllerSpecs
{
    [Subject("SAHL.Core.Web.Services.CommandHttpHandlerBaseController.PerformHttpCommand")]
    public class when_performing_command : WithFakes
    {
        private static CommandHttpHandlerBaseController commandController;
        private static IServiceCommandRouter commandRouter;
        private static ICommandServiceRequestMetrics commandMetrics;
        private static IHttpCommandAuthoriser commandAuthoriser;
        private static IJsonActivator jsonActivator;
        private static IMetadataManager metadataManager;
        private static IServiceCommand command;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ICommandSession commandSession;
        private static ICommandRetryPolicy commandRetryPolicy;
        private static ICommandSessionFactory commandSessionFactory;
        private const string contentString = "";
        private static HttpResponseMessage result;
        private static HttpCommandRun httpCommandRun;

        private Establish context = () =>
        {
            command = An<IServiceCommand>();
            jsonActivator = An<IJsonActivator>();
            commandAuthoriser = An<IHttpCommandAuthoriser>();
            metadataManager = An<IMetadataManager>();
            commandRouter = An<IServiceCommandRouter>();
            commandMetrics = An<ICommandServiceRequestMetrics>();
            logger = new FakeLogger();
            loggerSource = An<ILoggerSource>();
            commandSession = An<ICommandSession>();
            commandRetryPolicy = An<ICommandRetryPolicy>();

            var authToken = new HttpCommandAuthoriser.AuthToken();

            jsonActivator.WhenToldTo(x => x.DeserializeCommand(contentString)).Return(command);
            commandAuthoriser.WhenToldTo(x => x.AuthoriseCommand(command)).Return(authToken);
            commandRetryPolicy.WhenToldTo(x => x.ShouldRetry()).Return(false);

            commandSessionFactory = An<ICommandSessionFactory>();
            commandSessionFactory.WhenToldTo(x => x.CreateNewCommandManager("")).Return(commandSession);
            httpCommandRun = An<HttpCommandRun>(commandAuthoriser, jsonActivator, metadataManager, logger, commandRouter, commandMetrics, loggerSource);
            commandController = new CommandHttpHandlerBaseController(logger, loggerSource, commandSessionFactory, httpCommandRun, An<IJsonActivator>(), An<IHostContext>());
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
            commandAuthoriser.WasToldTo(x => x.AuthoriseCommand(command));
        };

        private It should_get_meta_data = () =>
        {
            metadataManager.WasToldTo(x => x.GetMetaData());
        };

        private It should_increment_metric_for_command = () =>
        {
            commandMetrics.WasToldTo(x => x.IncrementRequestForCommand<IServiceCommand>("HandleHttpCommand"));
        };

        private It should_tell_service_command_router_to_handle_command = () =>
        {
            commandRouter.WasToldTo(x => x.HandleCommand(Param<object>.IsAnything, Param<IServiceRequestMetadata>.IsAnything));
        };

        private It should_update_request_latency_metric = () =>
        {
            commandMetrics.WasToldTo(x => x.UpdateRequestLatencyForCommand<IServiceCommand>("HandleHttpCommand", Param<long>.IsAnything));
        };

        private It should_set_success_status_to_true_on_result = () =>
        {
            result.IsSuccessStatusCode.ShouldEqual(true);
        };

        private It should_persist_the_command = () =>
        {
            commandSession.WasToldTo(x => x.PersistCommand(""));
        };

        private It should_not_ask_for_retry = () =>
        {
            commandRetryPolicy.WasNotToldTo(x => x.ShouldRetry());
        };

        private It should_complete_the_command = () =>
        {
            commandSession.WasToldTo(x => x.CompleteCommandAuthorised());
        };
    }
}