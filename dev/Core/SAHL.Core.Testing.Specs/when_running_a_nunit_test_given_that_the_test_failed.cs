using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Ioc;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing;
using System;

namespace SAHL.Core.Testing.Specs
{
    public class when_running_a_nunit_test_given_that_the_test_failed : WithFakes
    {
        private static Action actualTest;
        private static IConventionTestSuite conventionTestSuite;
        private static Exception exception;
        private static ITestParams test;
        Establish context = () =>
        {
            test = Substitute.For<ITestParams>();
            test.WhenToldTo(x => x.TypeUnderTest).Return(typeof(when_running_a_nunit_test_given_that_the_test_failed));
            conventionTestSuite = new ConventionTestSuite();
            actualTest = new Action(() =>
            {
                throw new Exception("Test failed.");
            });
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => { conventionTestSuite.Run("SuiteName", "SuiteTest", test, actualTest); });
        };

        It should_report_back_the_invoking_assembly_name = () =>
        {
            const string message = "The test SuiteName.SuiteTest failed to execute in assembly {0}, \nThe error was \"Test failed.\", \nTotal Test Count: 1";
            var expectedMessage = string.Format(message, typeof(when_running_a_nunit_test_given_that_the_test_failed).Assembly.GetName());
            exception.Message.ShouldEqual(expectedMessage);
        };
    }
}
