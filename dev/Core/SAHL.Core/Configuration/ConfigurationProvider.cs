using System.Configuration;

namespace SAHL.Core.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public ConfigurationProvider()
        {
            if (System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.Contains("web.config"))
            {
                this.Config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
            else
            {
                this.Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
        }

        public System.Configuration.Configuration Config { get; private set; }
    }
}