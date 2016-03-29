using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_creating_a_loggersourcemanager_with_logger_sources : WithFakes
    {
        private static ILoggerSourceManager loggerSourceManager;
        private static ILoggerSource loggerSource;

        private Establish context = () =>
        {
            loggerSource = new LoggerSource("testsource", LogLevel.Info, true);
        };

        private Because of = () =>
        {
            loggerSourceManager = new LoggerSourceManager(new ILoggerSource[] { loggerSource });
        };

        private It should_register_the_loggersource = () =>
        {
            loggerSourceManager.AvailableSources.ShouldContain(kvp => kvp.Key == loggerSource.Id);
        };
    }
}