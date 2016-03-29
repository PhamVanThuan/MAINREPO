using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using System;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_registering_a_logger_source_with_the_same_name : WithFakes
    {
        private static ILoggerSourceManager loggerSourceManager;
        private static ILoggerSource loggerSource;
        private static ILoggerSource loggerSource2;
        private static Exception exception;

        private Establish context = () =>
        {
            loggerSourceManager = new LoggerSourceManager(new ILoggerSource[] { });
            loggerSource = new LoggerSource("testsource", LogLevel.Info, true);
            loggerSource2 = new LoggerSource("testsource", LogLevel.Info, true);
        };

        private Because of = () =>
        {
            loggerSourceManager.RegisterSource(loggerSource);
            exception = Catch.Exception(() => loggerSourceManager.RegisterSource(loggerSource2));
        };

        private It should_register_the_first_loggersource = () =>
        {
            loggerSourceManager.AvailableSources.ShouldContain(kvp => kvp.Key == loggerSource.Id);
        };

        private It should_throw_an_exception = () =>
        {
            exception.ShouldBeOfExactType<ArgumentException>();
        };

        private It should_not_register_the_second_loggersource = () =>
        {
            loggerSourceManager.AvailableSources.ShouldNotContain(kvp => kvp.Key == loggerSource2.Id);
        };

        private It should_indicate_the_logger_source_name_has_already_been_registered = () =>
        {
            exception.Message.ShouldContain("has already been registered");
            ((ArgumentException)exception).ParamName.ShouldEqual("loggersource");
        };
    }
}