using SAHL.Core.Services;
using SAHL.Core.Services.Metrics;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public class CommandHandlerWithReturnedDataController : CommandHandlerWithReturnedDataBaseController
    {
        public CommandHandlerWithReturnedDataController(IServiceCommandRouter serviceCommandRouter, ICommandServiceRequestMetrics commandServiceMetrics)
            : base(serviceCommandRouter, commandServiceMetrics)
        {
        }
    }
}