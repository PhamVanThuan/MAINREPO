using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionFactorySpecs
{
    public class when_getting_a_collection_assertion_given_should_contains : WithFakes
    {
        private static AssertionFactory factory;
        private static string assertion;
        private static CollectionAssertion result;

        private Establish context = () =>
        {
            factory = new AssertionFactory();
            assertion = "should contain";
        };

        private Because of = () =>
        {
            result = factory.GetCollectionAssertion(assertion);
        };

        private It should_return_a_collection_contains_assertion = () =>
        {
            result.ShouldBe(typeof(CollectionContainsAssertion));
        };
    }

    public class when_getting_a_collection_assertion_given_should_have_been_called : WithFakes
    {
        private static AssertionFactory factory;
        private static string assertion;
        private static CollectionAssertion result;

        private Establish context = () =>
        {
            factory = new AssertionFactory();
            assertion = "should have been called";
        };

        private Because of = () =>
        {
            result = factory.GetCollectionAssertion(assertion);
        };

        private It should_return_a_collection_contains_assertion = () =>
        {
            result.ShouldBe(typeof(CollectionContainsAssertion));
        };
    }

    public class when_getting_a_collection_assertion_given_should_not_contains : WithFakes
    {
        private static AssertionFactory factory;
        private static string assertion;
        private static CollectionAssertion result;

        private Establish context = () =>
        {
            factory = new AssertionFactory();
            assertion = "should not contain";
        };

        private Because of = () =>
        {
            result = factory.GetCollectionAssertion(assertion);
        };

        private It should_return_a_collection_not_contains_assertion = () =>
        {
            result.ShouldBe(typeof(CollectionNotContainsAssertion));
        };
    }

    public class when_getting_a_collection_assertion_given_should_not_have_been_called : WithFakes
    {
        private static AssertionFactory factory;
        private static string assertion;
        private static CollectionAssertion result;

        private Establish context = () =>
        {
            factory = new AssertionFactory();
            assertion = "should not have been called";
        };

        private Because of = () =>
        {
            result = factory.GetCollectionAssertion(assertion);
        };

        private It should_return_a_collection_not_contains_assertion = () =>
        {
            result.ShouldBe(typeof(CollectionNotContainsAssertion));
        };
    }

    public class when_getting_a_collection_assertion_given_should_be_empty : WithFakes
    {
        private static AssertionFactory factory;
        private static string assertion;
        private static CollectionAssertion result;

        private Establish context = () =>
        {
            factory = new AssertionFactory();
            assertion = "should be empty";
        };

        private Because of = () =>
        {
            result = factory.GetCollectionAssertion(assertion);
        };

        private It should_return_a_collection_empty_assertion = () =>
        {
            result.ShouldBe(typeof(CollectionEmptyAssertion));
        };
    }

   
}
