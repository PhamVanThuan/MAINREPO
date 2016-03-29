using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Services;
using SAHL.Services.Cuttlefish.Workers;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Cuttlefish.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                scanner.WithDefaultConventions();
                scanner.WithDefaultConventions();
            });

            For<ILoggerSource>().Singleton().Use(new LoggerSource("Cuttlefish.Service", LogLevel.Error, true)).Named("CuttlefishLogSource");

            For<IDbConnectionProvider>().Use<SqlServerConnectionProvider>();
            For<CuttlefishService>().Singleton().Use<CuttlefishService>().Named("CuttlefishService");
            For<IStartableService>().Use(x => x.GetInstance<CuttlefishService>("CuttlefishService"));
            For<IStoppableService>().Use(x => x.GetInstance<CuttlefishService>("CuttlefishService"));

            For<IQueueConsumerFactory>().Use<QueueConsumerFactory>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("CuttlefishLogSource"));

            For<IQueueConnectionFactory>().Use<QueueInstanceFactory>()
                .Ctor<ILoggerSource>().Is(x => x.GetInstance<ILoggerSource>("CuttlefishLogSource"));
        }
    }
}