using SAHL.Core.Services;

namespace SAHL.Core.Web.Services
{
    public class ServiceHttpClientWindowsAuthenticated : ServiceHttpClient
    {
        public ServiceHttpClientWindowsAuthenticated(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
            this.UseWindowsAuth();
        }
    }
}