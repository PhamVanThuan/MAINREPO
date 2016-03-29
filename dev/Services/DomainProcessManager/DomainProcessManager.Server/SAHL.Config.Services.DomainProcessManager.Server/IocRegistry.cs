using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap.Configuration.DSL;

using SAHL.Core.IoC;
using SAHL.Core.Data;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.DomainProcessManager.Data;
using SAHL.Services.DomainProcessManager.Services;
using SAHL.Services.DomainProcessManager.WcfService;

namespace SAHL.Config.Services.DomainProcessManager.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));

                    scanner.AddAllTypesOf(typeof(IUIStatementsProvider));
                    scanner.WithDefaultConventions();
                });

            this.For<IJsonActivator>().Use<JsonActivator>();

            For<ILoggerSource>().Singleton().Use(new LoggerSource("DomainProcess", LogLevel.Error, true)).Named("DomainProcessLogSource");

            this.For<IDomainProcessRepository>().Use<DomainProcessRepository>();
            this.For<IDomainProcessEventHandlerService>().Singleton()
                                                         .Use<DomainProcessEventHandlerService>()
                                                         .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("DomainProcessLogSource"));

            this.For<IDomainProcessCoordinatorService>().Singleton()
                                                        .Use<DomainProcessCoordinatorService>()
                                                        .Ctor<IDomainProcessRepository>().Is(x => x.GetInstance<IDomainProcessRepository>())
                                                        .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("DomainProcessLogSource"));

            this.For<IStartable>().Use<WcfServiceStartup>();
            this.For<IStoppable>().Use<WcfServiceStartup>();

            this.For<IStartable>().Use<DomainProcessEventHandlerService>();
            this.For<IStoppable>().Use<DomainProcessEventHandlerService>();

            this.For<IStartable>().Use<DomainProcessCoordinatorService>();
            this.For<IStoppable>().Use<DomainProcessCoordinatorService>();

            this.For<IStartableService>().Use<HostedService>();
        }
    }
}