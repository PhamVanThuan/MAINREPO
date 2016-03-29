using Capitec.Core.Identity;
using Capitec.Core.Identity.Web;
using SAHL.Core.Identity;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Web.Auth.Capitec
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IAuthenticationManager>().Use<AuthenticationManager>();
            For<IHostContext>().Use<HttpHostContext>();
        }
    }
}