using SAHL.Config.Services.Core;
using SAHL.Core.Services;
using System.Collections.Specialized;

namespace SAHL.Config.Services.Windows.Web
{
    public class WebSelfHostedServiceSettings : ServiceSettings, IWebSelfHostedServiceSettings
    {
        public WebSelfHostedServiceSettings(NameValueCollection nameValueCollection)
            : base(nameValueCollection)
        {
        }

        public string WebApiBaseAddress
        {
            get
            {
                var value = this.nameValueCollection["WebApiBaseAddress"];
                return value ?? "http://localhost:9001";
            }
        }
    }
}