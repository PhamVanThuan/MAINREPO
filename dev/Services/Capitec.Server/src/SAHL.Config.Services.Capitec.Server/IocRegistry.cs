using Capitec.Core.Identity;
using Capitec.Core.Identity.Web;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Managers.RequestPublisher;
using SAHL.Services.Interfaces.Communications;
using StructureMap.Configuration.DSL;
using System.Configuration;

namespace SAHL.Config.Services.Capitec.Server
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
            For<IEasyNetQMessageBusSettings>().Singleton().Use<DefaultEasyNetQMessageBusSettings>();
            For<IHostContext>().Use<HttpHostContext>().Named("CapitecHttpContext");
            For<ILookupManager>().Singleton().Use<LookupManager>();
           
            For<SAHL.Services.Capitec.Managers.RequestPublisher.IConfigurationProvider>().Singleton().Use<SAHL.Services.Capitec.Managers.RequestPublisher.ConfigurationProvider>();

            SetAllProperties(x => x.OfType<IAuthenticationManager>());

            FillAllPropertiesOfType<ISecurityModule>();

            // ITCManager Logger
            For<ILoggerSource>().Use(new LoggerSource("ITCManager", LogLevel.Error, true)).Named("ITCManagerLogSource");
            For<ITCManager>().Use<ITCManager>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("ITCServiceLogSource"));

            // Request Publisher Logger
            For<ILoggerSource>().Use(new LoggerSource("RequestPublisherManager", LogLevel.Error, true)).Named("RequestPublisherManagerLogSource");
            For<IRequestPublisherManager>().Use<RequestPublisherManager>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("RequestPublisherManagerLogSource"));
        }
    }
}