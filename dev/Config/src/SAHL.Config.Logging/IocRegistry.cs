using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using StructureMap.Configuration.DSL;
using System;

namespace SAHL.Config.Logging
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IRawLogger>().Singleton().Use<DatabaseLogger>();
            For<IRawMetricsLogger>().Singleton().Use<DatabaseMetricsLogger>();
            For<ILoggerSource>().Singleton().Use(new LoggerSource("DefaultLogSource", LogLevel.Error, false));
            For<ILoggerSource>().Singleton().Use(new LoggerSource("AppStartUp", LogLevel.Error, true)).Named("AppStartUpLogSource");
        }
    }
}