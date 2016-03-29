using SAHL.Core.Logging;

namespace SAHL.Config.Services.Core
{
    public class LoggerAppSourceFromServiceName : ILoggerAppSource
    {
        private IServiceSettings serviceSettings;

        public LoggerAppSourceFromServiceName(IServiceSettings serviceSettings)
        {
            this.serviceSettings = serviceSettings;
        }

        public string ApplicationName
        {
            get { return this.serviceSettings.ServiceName; }
        }
    }
}