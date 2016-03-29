using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_registering_a_logger_source : WithFakes
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
        };

        private It should_register_the_loggersource = () =>
        {
            loggerSourceManager.AvailableSources.ShouldContain(kvp => kvp.Key == loggerSource.Id);
        };
    }
}