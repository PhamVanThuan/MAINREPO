using SAHL.Core.Configuration;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Halo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Halo.Client
{
    public class HaloServiceConfigurationProvider : ConfigurationProvider, IHaloServiceConfigurationProvider
    {
        public string ServiceName
        {
            get { return this.Config.AppSettings.Settings["HaloServiceName"].Value; }
        }

        public string GetCommandServiceUrl()
        {
            return this.Config.AppSettings.Settings["HaloCommandServiceUrl"].Value;
        }
    }
}
