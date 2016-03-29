using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Web.Services;
using SAHL.Core.Services.CommandPersistence;

namespace SAHL.Services.WindowsWeb.CommandService
{
    public class CommandHttpHandlerController : CommandHttpHandlerBaseController
    {
        public CommandHttpHandlerController(ILogger logger, ILoggerSource loggerSource, ICommandSessionFactory commandSessionFactory, 
                                            IHttpCommandRun httpCommandRun, IJsonActivator jsonActivator, IHostContext hostContext)
            : base(logger, loggerSource, commandSessionFactory, httpCommandRun, jsonActivator, hostContext)
        {
        }
    }
}