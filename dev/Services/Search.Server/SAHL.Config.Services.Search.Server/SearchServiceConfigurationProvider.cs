using SAHL.Core.Configuration;
using SAHL.Services.Interfaces.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Search.Client
{
    public class SearchServiceConfigurationProvider : ConfigurationProvider, ISearchServiceConfigurationProvider
    {
        public string ServiceName
        {
            get { return this.Config.AppSettings.Settings["SearchServiceName"].Value; }
        }

        public string CommandServiceUrl
        {
            get { return this.Config.AppSettings.Settings["SearchCommandServiceUrl"].Value; }
        }

        public string CommandWithResultServiceUrl
        {
            get { return this.Config.AppSettings.Settings["SearchCommandWithResultServiceUrl"].Value; }
        }
    }
}
