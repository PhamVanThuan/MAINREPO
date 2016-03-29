using SAHL.Core.Configuration;
using System;

namespace SAHL.Core.Services
{
    public class ServiceUrlConfigurationProvider : ConfigurationProvider, IServiceUrlConfigurationProvider
    {
        private readonly string serviceName;

        public ServiceUrlConfigurationProvider(string serviceName)
        {
            if (serviceName == null) { throw new ArgumentNullException("serviceName"); }
            this.serviceName = serviceName;
        }

        public bool IsSelfHostedService
        {
            get
            {
                var settingName = string.Format("{0}_IsSelfHosted", serviceName);
                return this.Config.AppSettings.Settings[settingName] != null && Convert.ToBoolean(this.Config.AppSettings.Settings[settingName].Value);
            }
        }

        public string ServiceHostName
        {
            get
            {
                var settingName = string.Format("{0}_HostName", serviceName);
                if (this.Config.AppSettings.Settings[settingName] == null)
                {
                    throw new Exception(string.Format("Service Setting {0} not found", settingName));
                }

                return this.Config.AppSettings.Settings[settingName].Value;
            }
        }

        public string ServiceName
        {
            get
            {
                var settingName = string.Format("{0}_ServiceName", serviceName);
                return this.Config.AppSettings.Settings[settingName] == null
                                ? string.Empty
                                : this.Config.AppSettings.Settings[settingName].Value;
            }
        }
    }
}