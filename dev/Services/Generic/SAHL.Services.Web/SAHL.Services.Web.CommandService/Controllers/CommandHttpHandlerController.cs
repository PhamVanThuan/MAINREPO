using SAHL.Core.Logging;
using SAHL.Core.Identity;
using SAHL.Core.Web.Services;
using SAHL.Core.Services.CommandPersistence;

namespace SAHL.Services.Web.CommandService.Controllers
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