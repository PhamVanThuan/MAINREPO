using SAHL.Config.Services.Core;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Windows.Web
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IWebApiService>().Singleton().Use<WebApiService>();

            For<IStartableService>().Use(y => y.GetInstance<IWebApiService>() as IStartableService);
            For<IStoppableService>().Use(y => y.GetInstance<IWebApiService>() as IStoppableService);

            For<IWebSelfHostedServiceSettings>().Use<WebSelfHostedServiceSettings>();
            For<IServiceSettings>().Use(y => y.GetInstance<IWebSelfHostedServiceSettings>());

            For<ILoggerSource>().Singleton().Use(new LoggerSource("CommandHandler", LogLevel.Error, true)).Named("CommandHandlerLogSource");
            For<ILoggerSource>().Singleton().Use(new LoggerSource("QueryHandler", LogLevel.Error, true)).Named("QueryHandlerLogSource");

            For<QueryHttpHandlerBaseController>().Use<QueryHttpHandlerBaseController>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("QueryHandlerLogSource"));

            For<CommandHttpHandlerBaseController>().Use<CommandHttpHandlerBaseController>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("CommandHandlerLogSource"));
        }
    }
}