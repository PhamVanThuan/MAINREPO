using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Collections.Generic;

using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Websites.Halo.Shared
{
    public class HaloHttpServiceClient : ServiceHttpClient
    {
        private static readonly ServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration(WebConfigurationManager.AppSettings["HaloService"]);
        private static readonly IJsonActivator jsonActivator                 = new JsonActivator();

        public static HaloHttpServiceClient Instance()
        {
            return new HaloHttpServiceClient(serviceConfiguration, jsonActivator);
        }

        private HaloHttpServiceClient(IServiceUrlConfiguration serviceConfiguration, IJsonActivator jsonActivator)
            : base(serviceConfiguration, jsonActivator)
        {
            this.UseWindowsAuth();
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
