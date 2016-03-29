using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services.CommandPersistence;
using System;

namespace SAHL.Core.Web.Services
{
    public class HttpCommandReRun : IHttpCommandReRun
    {
        private readonly IHostContextHelper contextHelper;
        private readonly IIocContainer iocContainer;
        private readonly ILoggerSource loggerSource;
        private readonly ILogger logger;

        public HttpCommandReRun(IHostContextHelper contextHelper, IIocContainer iocContainer, ILoggerSource loggerSource, ILogger logger)
        {
            this.contextHelper = contextHelper;
            this.iocContainer = iocContainer;
            this.loggerSource = loggerSource;
            this.logger = logger;
        }

        public ServiceCommandResult TryRunCommand(ICommandSession commandSession)
        {
            IHostContext context = null;
            ServiceCommandResult serviceResult = new ServiceCommandResult();
            try
            {
                context = contextHelper.CreateHostContextFromUser(commandSession.RunAsUsername, commandSession.ContextDetails);

                IHttpCommandRun commandRunner = iocContainer.GetInstance<IHttpCommandRun, IHostContext>(context);
                commandRunner.RunCommand(serviceResult, commandSession);
            }
            catch (Exception ex)
            {
                logger.LogErrorWithException(loggerSource, commandSession.RunAsUsername, "HttpCommandReRun", "Exception while performing an http command", ex);
                throw;
            }
            return serviceResult;
        }
    }
}