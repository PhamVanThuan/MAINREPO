using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionSpecs
{
    public class when_asserting_a_collection_is_empty_given_the_collection_contains_a_value : WithFakes
    {
        private static CollectionEmptyAssertion assertion;
        private static List<string> expected;
        private static bool result;

        private Establish context = () =>
        {
            assertion = new CollectionEmptyAssertion();
            expected = new List<string> { "Item one", "Item two", "Item three" };
        };

        private Because of = () =>
        {
            result = assertion.Assert(expected, "");
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }

    public class when_asserting_a_collection_is_empty_given_the_collection_does_not_contain_a_value : WithFakes
    {
        private static CollectionEmptyAssertion assertion;
        private static List<string> expected;
        private static bool result;

        private Establish context = () =>
        {
            assertion = new CollectionEmptyAssertion();
            expected = new List<string>();
        };

        private Because of = () =>
        {
            result = assertion.Assert(expected, "");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
