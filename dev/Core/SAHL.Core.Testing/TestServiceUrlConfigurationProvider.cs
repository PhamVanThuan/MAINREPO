using SAHL.Core.Services;

namespace SAHL.Core.Testing
{
    public class TestServiceUrlConfigurationProvider : IServiceUrlConfigurationProvider
    {
        public TestServiceUrlConfigurationProvider(bool isSelfHosted, string serviceHostName, string serviceName)
        {
            this.IsSelfHostedService = isSelfHosted;
            this.ServiceHostName = serviceHostName;
            this.ServiceName = serviceName;
        }

        public bool IsSelfHostedService
        {
            get;
            protected set;
        }

        public string ServiceHostName
        {
            get;
            protected set;
        }

        public string ServiceName
        {
            get;
            protected set;
        }

        public System.Configuration.Configuration Config
        {
            get { return null; }
        }
    }
}