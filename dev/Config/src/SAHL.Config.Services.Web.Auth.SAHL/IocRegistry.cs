using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.Identity;
using SAHL.Core.Web.Identity;
using SAHL.Core.Web.Services;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Web.Auth.SAHL
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IHttpCommandAuthoriser>().Use<HttpCommandAuthoriser>();
            For<ICredentials>().Use<DefaultCredentials>();
            For<IHostContext>().Use<HttpHostContext>();
        }
    }
}