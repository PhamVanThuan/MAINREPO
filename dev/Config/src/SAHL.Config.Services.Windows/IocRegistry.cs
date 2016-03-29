using SAHL.Config.Services.Core;
using SAHL.Core.Logging;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Windows
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<ILoggerSource>().Singleton().Use(new LoggerSource("DefaultLogSource", LogLevel.Info, false));
            For<ILoggerSource>().Singleton().Use(new LoggerSource("AppStartUp", LogLevel.Info, true)).Named("AppStartUpLogSource");

            For<IWindowsServiceSettings>().Use<WindowsServiceSettings>();
            For<IServiceSettings>().Use(context => context.GetInstance<IWindowsServiceSettings>());
        }
    }
}