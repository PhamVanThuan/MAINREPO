using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.V3.Client
{
    public class ServiceUrlConfigurationProvider
    {
        private readonly string serviceName;

        public ServiceUrlConfigurationProvider(string serviceName)
        {
            if (serviceName == null) { throw new ArgumentNullException("serviceName"); }
            this.serviceName = serviceName;
        }

        public string ServiceHostName
        {
            get
            {
                var settingName = string.Format("{0}_HostName", serviceName);
                if (ConfigurationManager.AppSettings[settingName] == null)
                {
                    throw new Exception(string.Format("Service Setting {0} not found", settingName));
                }

                return ConfigurationManager.AppSettings[settingName];
            }
        }

        public string ServiceName
        {
            get
            {
                var settingName = string.Format("{0}_ServiceName", serviceName);
                if (ConfigurationManager.AppSettings[settingName] == null)
                {
                    throw new Exception(string.Format("Service Setting {0} not found", settingName));
                }

                return ConfigurationManager.AppSettings[settingName];
            }
        }
    }
}
