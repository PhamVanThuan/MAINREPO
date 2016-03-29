using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.ITC.Managers.Itc;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.ITC.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                x.WithDefaultConventions();
            });
            // ITCManager Logger
            For<ILoggerSource>().Use(new LoggerSource("ITCManager", LogLevel.Error, true)).Named("ITCManagerLogSource");
            For<ItcManager>().Use<ItcManager>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("ITCServiceLogSource"));

            For<IDomainRuleManager<PerformClientITCCheckCommand>>().Use<DomainRuleManager<PerformClientITCCheckCommand>>();
        }
    }
}
