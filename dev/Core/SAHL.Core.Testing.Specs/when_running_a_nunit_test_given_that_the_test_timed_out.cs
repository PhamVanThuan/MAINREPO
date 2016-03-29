using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Ioc;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing;
using System;
using System.Threading;

namespace SAHL.Core.Testing.Specs
{
    public class when_running_a_nunit_test_given_that_the_test_timed_out : WithFakes
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
            actualTest= new Action(()=>{
                Thread.Sleep(11000);
            });
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => { 
                conventionTestSuite.Run("SuiteName", "SuiteTest", test, actualTest); 
            });
        };

        It should_fail_with_timeout_error = () =>
        {
            var expectedMessage = String.Format("The test SuiteName.SuiteTest failed to execute in assembly {0}, \nThe error was", 
                                                typeof(when_running_a_nunit_test_given_that_the_test_failed).Assembly.GetName());
            exception.Message.ShouldContain(expectedMessage);
        };
    }
}
