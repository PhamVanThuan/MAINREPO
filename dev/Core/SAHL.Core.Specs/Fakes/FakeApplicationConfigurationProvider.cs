using SAHL.Core.Configuration;

namespace SAHL.Core.Specs.Fakes
{
    public class FakeApplicationConfigurationProvider : IApplicationConfigurationProvider
    {
        public FakeApplicationConfigurationProvider(string applicationName)
        {
            this.ApplicationName = applicationName;
        }

        public string ApplicationName
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