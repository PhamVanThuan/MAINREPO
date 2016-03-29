using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionFactorySpecs
{
    public class when_getting_an_assertion_given_to_equal : WithFakes
    {
        private static AssertionFactory factory;
        private static string assertion;
        private static Assertion result;

        private Establish context = () =>
        {
            factory = new AssertionFactory();
            assertion = "to equal";
        };

        private Because of = () =>
        {
            result = factory.GetAssertion(assertion);
        };

        private It should_return_a_collection_contains_assertion = () =>
        {
            result.ShouldBe(typeof(EqualsAssertion));
        };
    }
}