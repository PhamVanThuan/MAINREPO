using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.Metrics;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public class QueryHttpHandlerController : QueryHttpHandlerBaseController
    {
        public QueryHttpHandlerController(IServiceQueryRouter serviceQueryRouter, ICommandServiceRequestMetrics commandServiceMetrics, 
                                          IHttpCommandAuthoriser authoriser, IJsonActivator jsonQueryActivator, IQueryParameterManager queryParameterManager, 
                                          ILogger logger, ILoggerSource loggerSource)
            : base(serviceQueryRouter, commandServiceMetrics, authoriser, jsonQueryActivator, queryParameterManager, logger, loggerSource)
        {
        }
    }
}