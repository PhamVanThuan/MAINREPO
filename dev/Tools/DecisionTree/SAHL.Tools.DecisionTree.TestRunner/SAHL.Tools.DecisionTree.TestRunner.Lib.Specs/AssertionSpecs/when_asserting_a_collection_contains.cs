using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionSpecs
{
    public class when_asserting_a_collection_contains_given_the_collection_contains_the_value : WithFakes
    {
        private static CollectionContainsAssertion assertion;
        private static List<string> expected;
        private static string actual;
        private static bool result;

        private Establish context = () =>
        {
            assertion = new CollectionContainsAssertion();
            expected = new List<string> { "Item one", "Item two", "Item three" };
            actual = "Item two";
        };

        private Because of = () =>
        {
            result = assertion.Assert(expected, actual);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }

    public class when_asserting_a_collection_contains_given_the_collection_does_not_contain_the_value : WithFakes
    {
        private static CollectionContainsAssertion assertion;
        private static List<string> expected;
        private static string actual;
        private static bool result;

        private Establish context = () =>
        {
            assertion = new CollectionContainsAssertion();
            expected = new List<string> { "Item one", "Item two", "Item three" };
            actual = "Item six";
        };

        private Because of = () =>
        {
            result = assertion.Assert(expected, actual);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
