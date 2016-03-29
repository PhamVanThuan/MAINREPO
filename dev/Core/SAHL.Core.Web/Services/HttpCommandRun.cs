using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Services.Metrics;
using SAHL.Core.SystemMessages;
using System;
using System.Diagnostics;
using System.Net;

namespace SAHL.Core.Web.Services
{
    public class HttpCommandRun : IHttpCommandRun
    {
        private readonly IHttpCommandAuthoriser authoriser;
        private readonly IJsonActivator jsonQueryActivator;
        private readonly IMetadataManager httpMetaManager;
        private readonly ILogger logger;
        private readonly IServiceCommandRouter serviceCommandRouter;
        private readonly ICommandServiceRequestMetrics commandServiceMetrics;
        private readonly ILoggerSource loggerSource;

        public HttpCommandRun(IHttpCommandAuthoriser authoriser, IJsonActivator jsonQueryActivator, IMetadataManager httpMetaManager,
            ILogger logger, IServiceCommandRouter serviceCommandRouter, ICommandServiceRequestMetrics commandServiceMetrics, ILoggerSource loggerSource)
        {
            this.authoriser = authoriser;
            this.jsonQueryActivator = jsonQueryActivator;
            this.httpMetaManager = httpMetaManager;
            this.logger = logger;
            this.serviceCommandRouter = serviceCommandRouter;
            this.commandServiceMetrics = commandServiceMetrics;
            this.loggerSource = loggerSource;
        }

        public HttpStatusCode RunCommand(ServiceCommandResult result, ICommandSession commandSession)
        {
            try
            {
                string json = commandSession.Command.CommandJson;
                var serviceCommand = jsonQueryActivator.DeserializeCommand(json);
                HttpCommandAuthoriser.AuthToken authToken = authoriser.AuthoriseCommand(serviceCommand);

                HttpStatusCode returnCode = authToken.IsAuthorised() ?
                    ActionCommand(result, commandSession, serviceCommand) :
                    FindWhyAuthTokenIsNotAuthorised(result, commandSession, authToken);

                //for debug purposes keep this in a seperate variable so there is no need to dive into methods to find code.
                return returnCode;
            }
            catch (Exception exception)
            {
                if (commandSession.CommandRetryPolicy.ShouldRetry())
                {
                    return RunCommand(result, commandSession);
                }
                commandSession.FailCommand(exception.Message);
                throw;
            }
        }

        private HttpStatusCode ActionCommand(ServiceCommandResult result, ICommandSession commandSession,
            IServiceCommand serviceCommand)
        {
            IServiceRequestMetadata metadata = httpMetaManager.GetMetaData();
            InternalCommand(serviceCommand as dynamic, result, metadata);

            var returnCode = HttpStatusCode.OK;

            if (result.SystemMessages.HasErrors)
            {
                returnCode = HttpStatusCode.PreconditionFailed;
            }
            else if(result.SystemMessages.HasExceptions)
            {
                returnCode = HttpStatusCode.InternalServerError;
            }
            commandSession.CompleteCommandAuthorised();
            return returnCode;
        }

        private static HttpStatusCode FindWhyAuthTokenIsNotAuthorised(ServiceCommandResult result, ICommandSession commandSession,
            HttpCommandAuthoriser.AuthToken authToken)
        {
            var returnCode = HttpStatusCode.OK;
            if (!authToken.Authenticated)
            {
                result.SystemMessages.AddMessage(new SystemMessage("User authentication required.",
                    SystemMessageSeverityEnum.Error));
                returnCode = HttpStatusCode.Unauthorized;
                commandSession.MarkUnAuthenticated();
                commandSession.CompleteCommand();
            }
            else if (!authToken.Authorised)
            {
                result.SystemMessages.AddMessage(
                    new SystemMessage("User has insufficient privilege to perform requested operation.",
                        SystemMessageSeverityEnum.Error));
                returnCode = HttpStatusCode.MethodNotAllowed;
                commandSession.MarkUnAuthorized();
                commandSession.CompleteCommand();
            }
            return returnCode;
        }

        private void InternalCommand<T>(T serviceCommand, ServiceCommandResult result, IServiceRequestMetadata metadata) where T : IServiceCommand
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            commandServiceMetrics.IncrementRequestForCommand<T>("HandleHttpCommand");

            this.logger.LogMethodMetric(this.loggerSource, metadata.UserName, "PerformHttpCommand", () =>
            {
                result.SystemMessages.Aggregate(this.serviceCommandRouter.HandleCommand((object)serviceCommand, metadata));
            });

            stopWatch.Stop();
            commandServiceMetrics.UpdateRequestLatencyForCommand<T>("HandleHttpCommand", stopWatch.ElapsedMilliseconds);
        }
    }
}