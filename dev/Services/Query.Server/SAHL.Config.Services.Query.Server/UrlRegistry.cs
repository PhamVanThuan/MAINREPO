using SAHL.Core.Identity;
using SAHL.Core.Strings;
using SAHL.Core.Web.Identity;
using SAHL.Services.Query.Url;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Query.Server
{
    public class UrlRegistry : Registry
    {
        public UrlRegistry()
        {
            this.For<IHostContext>()
                .Singleton() //seems to only be 'static HttpContext' calls inside?
                .Use<HttpHostContext>();

            this.For<IStringReplacer>()
                .Singleton()
                .Use<StringReplacer>();

            this.For<IUrlParameterSubstituter>()
                .Singleton()
                .Use<UrlParameterSubstituter>();
        }
    }
}
