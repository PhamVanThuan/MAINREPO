using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_unregistering_a_logger_source : WithFakes
    {
        private static ILoggerSourceManager loggerSourceManager;
        private static ILoggerSource loggerSource;

        private Establish context = () =>
        {
            loggerSourceManager = new LoggerSourceManager(new ILoggerSource[] { });
            loggerSource = new LoggerSource("testsource", LogLevel.Info, true);
        };

        private Because of = () =>
        {
            loggerSourceManager.RegisterSource(loggerSource);
            loggerSourceManager.UnregisterSource(loggerSource);
        };

        private It should_unregister_the_loggersource = () =>
        {
            loggerSourceManager.AvailableSources.ShouldNotContain(kvp => kvp.Key == loggerSource.Id);
        };
    }
}