using SAHL.Core.Configuration;

namespace SAHL.Core.Logging
{
    public class LoggerAppSourceFromConfiguration : ILoggerAppSource
    {
        private IApplicationConfigurationProvider applicationConfigurationProvider;

        public LoggerAppSourceFromConfiguration(IApplicationConfigurationProvider applicationConfigurationProvider)
        {
            this.applicationConfigurationProvider = applicationConfigurationProvider;
        }

        public string ApplicationName
        {
            get { return this.applicationConfigurationProvider.ApplicationName; }
        }
    }
}