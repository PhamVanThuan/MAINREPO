using SAHL.Core.Identity;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Web.Identity;
using SAHL.Core.Web.Services;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Web
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IWebService>().Singleton().Use<WebService>();

            For<IStartableService>().Use(y => y.GetInstance<IWebService>() as IStartableService);
            For<IStoppableService>().Use(y => y.GetInstance<IWebService>() as IStoppableService);

            For<ILoggerSource>().Singleton().Use(new LoggerSource("CommandHandler", LogLevel.Error, true)).Named("CommandHandlerLogSource");
            For<ILoggerSource>().Singleton().Use(new LoggerSource("QueryHandler", LogLevel.Error, true)).Named("QueryHandlerLogSource");

            For<QueryHttpHandlerBaseController>().Use<QueryHttpHandlerBaseController>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("QueryHandlerLogSource"));

            For<CommandHttpHandlerBaseController>().Use<CommandHttpHandlerBaseController>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("CommandHandlerLogSource"));
        }
    }
}