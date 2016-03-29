using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionManagerSpecs
{
    public class when_asserting_output_variable_test_passed : WithFakes
    {
        private static AssertionManagerTestFactory testFactory;
        private static IAssertionFactory assertionFactory;
        private static AssertionManager manager;

        private static TestOutput expected;
        private static int actualAge;
        private static string assertion;

        private static TestOutputResult result;
        private static FakeAssertion fakeAssertion;

        private Establish context = () =>
        {
            testFactory = new AssertionManagerTestFactory();
            manager = testFactory.AssertionManager;
            assertionFactory = testFactory.AssertionFactory;

            expected = new TestOutput("Age", "to equal", "int", "36");
            actualAge = 29;

            fakeAssertion = An<FakeAssertion>();
            assertionFactory.WhenToldTo(x => x.GetAssertion("to equal")).Return(fakeAssertion);
        };

        private Because of = () =>
        {
            result = manager.AssertOutputTestPassed(expected, 36, actualAge, typeof(Int32));
        };

        private It should_create_an_assertion_for_the_expectation = () =>
        {
            assertionFactory.WasToldTo(x => x.GetAssertion("to equal"));
        };

        private It should_call_assert_on_the_returned_assertion_with_output_values = () =>
        {
            fakeAssertion.WasToldTo(x => x.Assert(36, 29, typeof(Int32)));
        };

        private It should_return_the_assertion_result = () =>
        {
            result.Passed.ShouldBeFalse();
            result.Name.ShouldEqual("Age");
            result.Expected.ShouldEqual("36");
            result.Actual.ShouldEqual("29");
            result.Assertion.ShouldEqual("to equal");
        };
    }
}