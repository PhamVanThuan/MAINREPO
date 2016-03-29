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
    public class UserProfileHttpServiceClient : ServiceHttpClient
    {
        private static ServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration(WebConfigurationManager.AppSettings["UserProfileService"]);
        private static IJsonActivator jsonActivator                 = new JsonActivator();

        public static UserProfileHttpServiceClient Instance()
        {
            return new UserProfileHttpServiceClient(serviceConfiguration, jsonActivator);
        }

        private UserProfileHttpServiceClient(IServiceUrlConfiguration serviceConfiguration, IJsonActivator jsonActivator)
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
