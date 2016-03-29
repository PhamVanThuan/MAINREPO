using Capitec.Core.Identity;
using Capitec.Core.Identity.Web;
using SAHL.Core.Identity;
using SAHL.Core.TextSearch;
using SAHL.Core.TextSearch.Lucene;
using SAHL.Core.TextSearch.Lucene.Configuration;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.CapitecSearch.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("Capitec"));
                x.WithDefaultConventions();
            });

            For<IHostContext>().Use<HttpHostContext>();
            For<ILuceneTextSearchConfigurationProvider>().Use<LuceneConfigurationProvider>();
            For<ITextSearchProvider>().Use<LuceneTextSearch>();

            SetAllProperties(x => x.OfType<IAuthenticationManager>());

            FillAllPropertiesOfType<ISecurityModule>();
        }
    }
}