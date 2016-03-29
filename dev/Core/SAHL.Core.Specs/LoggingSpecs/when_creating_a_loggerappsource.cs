using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Configuration;
using SAHL.Core.Logging;
using SAHL.Core.Specs.Fakes;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_creating_a_loggerappsource : WithFakes
    {
        private static ILoggerAppSource loggerAppSource;
        private static IApplicationConfigurationProvider appConfigProvider;
        private static string appName;

        private Establish context = () =>
        {
            appName = "TestAppName";
            appConfigProvider = new FakeApplicationConfigurationProvider(appName);
        };

        private Because of = () =>
        {
            loggerAppSource = new LoggerAppSourceFromConfiguration(appConfigProvider);
        };

        private It should_register_the_loggersource = () =>
        {
            loggerAppSource.ApplicationName.ShouldEqual(appName);
        };
    }
}