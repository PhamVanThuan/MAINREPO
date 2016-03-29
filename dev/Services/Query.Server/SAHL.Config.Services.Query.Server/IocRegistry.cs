using Omu.ValueInjecter;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Helper;
using StructureMap.Configuration.DSL;
using System.Diagnostics;

namespace SAHL.Config.Services.Query.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.For<ILogger>().Use<Logger>();
            this.For<ILoggerSource>()
                .Singleton()
                .Use(new LoggerSource("Query Service", LogLevel.Error, false));

            this.For<ILookupTypesHelper>()
                .Singleton()
                .Use<LookupTypesHelper>();

            //would there be other IStartables?
            this.For<IStartable>().Add<LookupTypeStartable>();

            this.For<IValueInjecter>()
                .Singleton() //appears to be threadsafe
                .Use<ValueInjecter>();

            this.For<IQueryCoordinator>()
                .Singleton()
                .Use<QueryCoordinator>();
        }
    }
}