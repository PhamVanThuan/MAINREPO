using System;
using System.Threading;

using Microsoft.Owin.Hosting;

using SAHL.Core;
using SAHL.Core.Logging;

namespace SAHL.Config.Services.Windows.Web
{
    public class WebApiService : IWebApiService
    {
        private readonly IWebSelfHostedServiceSettings serviceSettings;
        private readonly IIocContainer iocContainer;
        private readonly ILogger logger;
        private readonly ILoggerSource loggerSource;

        private IDisposable webApiService;

        public WebApiService(IWebSelfHostedServiceSettings serviceSettings, IIocContainer iocContainer, ILogger logger, ILoggerSource loggerSource)
        {
            if (serviceSettings == null) { throw new ArgumentNullException("serviceSettings"); }
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            if (logger == null) { throw new ArgumentNullException("logger"); }
            if (loggerSource == null) { throw new ArgumentNullException("loggerSource"); }

            this.serviceSettings = serviceSettings;
            this.iocContainer    = iocContainer;
            this.logger          = logger;
            this.loggerSource    = loggerSource;
        }

        public void Dispose()
        {
            if (this.webApiService != null) { this.webApiService.Dispose(); }
        }

        public void Start()
        {
            Console.WriteLine("Starting Web API");

            var userName = Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null ? Thread.CurrentPrincipal.Identity.Name : "Uknown";

            this.logger.LogStartup(this.loggerSource, userName, "WebApiService.Start", "Starting WebApi Startable Service...");
            this.logger.LogStartup(this.loggerSource, userName, "WebApiService.Start", string.Format("WebApi Base Address [{0}]", serviceSettings.WebApiBaseAddress));

            try
            {
                this.webApiService = WebApp.Start<WebApiStartup>(url: serviceSettings.WebApiBaseAddress);
            }
            catch(Exception ex)
            {
                var errorMessage = string.Format("Error while starting webapi service\n", ex);
                this.logger.LogStartup(this.loggerSource, userName, "WebApiService.Start", errorMessage);
                throw;
            }

            this.logger.LogStartup(this.loggerSource, userName, "WebApiService.Start", "Starting WebApi Startable Service...Done");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping Web API");
            if (this.webApiService != null) { this.webApiService.Dispose(); }
        }
    }
}
