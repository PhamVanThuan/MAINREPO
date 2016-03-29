using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Logging.Commands;
using SAHL.Services.Logging.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Logging.Server.Specs.CommandHandlers
{
    public class when_logging_an_error_message : WithFakes
    {
        private static LogErrorCommand command;
        private static LogErrorCommandHandler handler;
        private static ILogger logger;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static Dictionary<string, string> metaData;
        private static bool shouldLogMetrics;

        private Establish context = () =>
        {
            logger = An<ILogger>();
            metaData = new Dictionary<string, string>();
            metaData.Add("UserName", @"SAHL\ClintonS");
            serviceRequestMetadata = new ServiceRequestMetadata(metaData);
            handler = new LogErrorCommandHandler(logger);
            command = new LogErrorCommand("Source", "Error Message", "StackTraceStackTraceStackTrace");
            shouldLogMetrics = false;
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_set_the_logger_source_level_to_error = () =>
        {
            logger.WasToldTo(x => x.LogFormattedError(Arg.Is<ILoggerSource>(y => y.LogLevel == LogLevel.Error), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()));
        };

        private It should_set_the_logger_source_name_to_the_command_source = () =>
        {
            logger.WasToldTo(x => x.LogFormattedError(Arg.Is<ILoggerSource>(y => y.Name == command.Source), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()));
        };

        private It should_not_log_metrics = () =>
        {
            logger.WasToldTo(x => x.LogFormattedError(Arg.Is<ILoggerSource>(y => y.LogMetrics == shouldLogMetrics), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()));
        };

        private It should_use_the_request_metadata_user_as_the_log_user = () =>
        {
            logger.WasToldTo(x => x.LogFormattedError(Arg.Any<ILoggerSource>(), serviceRequestMetadata.UserName, Arg.Any<string>(), Arg.Any<string>()));
        };

        private It should_log_the_message_provided_by_the_command = () =>
        {
            logger.WasToldTo(x => x.LogFormattedError(Arg.Any<ILoggerSource>(), Arg.Any<string>(), command.Message, Arg.Any<string>()));
        };

        private It should_log_the_stack_trace_provided_by_the_command = () =>
        {
            logger.WasToldTo(x => x.LogFormattedError(Arg.Any<ILoggerSource>(), Arg.Any<string>(), Arg.Any<string>(), command.StackTrace));
        };

        private It should_not_return_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}