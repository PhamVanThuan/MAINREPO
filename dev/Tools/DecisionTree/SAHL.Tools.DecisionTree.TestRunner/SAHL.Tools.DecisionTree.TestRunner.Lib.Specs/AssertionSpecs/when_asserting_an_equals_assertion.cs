using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionSpecs
{
    public class when_asserting_on_an_equals_assertion_given_equal : WithFakes
    {
        private static EqualsAssertion assertion;
        private static object expected;
        private static object actual;
        private static Type type;
        private static bool result;

        private Establish context = () =>
        {
            assertion = new EqualsAssertion();
            expected = "29";
            actual = 29;
            type = typeof(Int32);
        };

        private Because of = () =>
        {
            result = assertion.Assert(expected, actual, type);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }

    public class when_asserting_on_an_equals_assertion_given_not_equal : WithFakes
    {
        private static EqualsAssertion assertion;
        private static object expected;
        private static object actual;
        private static Type type;
        private static bool result;

        private Establish context = () =>
        {
            assertion = new EqualsAssertion();
            expected = "32";
            actual = 29;
            type = typeof(Int32);
        };

        private Because of = () =>
        {
            result = assertion.Assert(expected, actual, type);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
