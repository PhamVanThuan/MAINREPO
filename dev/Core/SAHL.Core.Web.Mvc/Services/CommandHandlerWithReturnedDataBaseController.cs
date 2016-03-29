using SAHL.Core.Services;
using SAHL.Core.Services.Metrics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

namespace SAHL.Core.Web.Services
{
    public class CommandHandlerWithReturnedDataBaseController : ApiController
    {
        private readonly IServiceCommandRouter serviceCommandRouter;
        private readonly ICommandServiceRequestMetrics commandServiceMetrics;

        public CommandHandlerWithReturnedDataBaseController(IServiceCommandRouter serviceCommandRouter, ICommandServiceRequestMetrics commandServiceMetrics)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.commandServiceMetrics = commandServiceMetrics;
        }

        [HttpPost]
        public ServiceQueryResult PerformCommandWithResult([FromBody] IServiceCommandWithReturnedData serviceCommand)
        {
            ServiceQueryResult result = new ServiceQueryResult();
            try
            {
                InternalCommand(serviceCommand as dynamic, result);
            }
            catch (Exception ex)
            {
                var allErrors = string.Join(";", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.Exception.ToString()));
                throw new Exception(allErrors, ex);
            }
            return result;
        }

        private void InternalCommand<T>(T serviceCommand, ServiceQueryResult result) where T : IServiceCommandWithReturnedData
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            commandServiceMetrics.IncrementRequestForCommand<T>("HandleCommand");

            result.SystemMessages.Aggregate(this.serviceCommandRouter.HandleCommand((object)serviceCommand, null));
            dynamic serviceCommandDynamic = serviceCommand;
            if (serviceCommandDynamic.Result != null)
            {
                result.SetReturnData(serviceCommandDynamic.Result);
            }

            stopWatch.Stop();
            commandServiceMetrics.UpdateRequestLatencyForCommand<T>("HandleCommand", stopWatch.ElapsedMilliseconds);
        }
    }
}