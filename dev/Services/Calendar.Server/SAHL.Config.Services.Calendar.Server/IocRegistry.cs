using Quartz;
using Quartz.Impl;
using SAHL.Core.IoC;
using SAHL.Core.Services;
using SAHL.Services.Calendar;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Calendar.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                scanner.Convention<JobConfigurationConvention>();
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
                scanner.LookForRegistries();
            });

            For<ICalendarService>().Singleton().Use<CalendarService>().Named("CalendarService");
            For<IStartable>().Use(x => x.GetInstance<CalendarService>("CalendarService"));
            For<IStoppable>().Use(x => x.GetInstance<CalendarService>("CalendarService"));

            For<ISchedulerFactory>().Use<StdSchedulerFactory>();

            For<IStartableService>().Use<HostedService>();
        }
    }
}
