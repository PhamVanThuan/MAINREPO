using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.RequestPublisher
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public int NumberOfTimesToRetryCreateApplication
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfTimesToRetryCreateApplication"]);
            }
        }
    }
}
