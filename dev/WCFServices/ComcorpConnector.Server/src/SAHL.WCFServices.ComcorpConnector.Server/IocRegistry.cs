using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;

namespace SAHL.WCFServices.ComcorpConnector.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<NameValueCollection>().Use(ConfigurationManager.AppSettings);
            IncludeRegistry<Config.Data.Dapper.IocRegistry>();
        }
    }
}