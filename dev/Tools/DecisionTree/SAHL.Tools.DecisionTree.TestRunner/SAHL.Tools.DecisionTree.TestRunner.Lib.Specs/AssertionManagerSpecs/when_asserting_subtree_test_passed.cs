using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionManagerSpecs
{
    public class when_asserting_subtree_test_passed : WithFakes
    {
        private static AssertionManagerTestFactory testFactory;
        private static IAssertionFactory assertionFactory;
        private static AssertionManager manager;
        private static ISubtreeExpectation subtreeExpectation;
        private static List<string> calledSubtrees;
        private static SubtreeExpectationResult result;
        private static CollectionAssertion fakeAssertion;

        private Establish context = () =>
        {
            testFactory = new AssertionManagerTestFactory();
            manager = testFactory.AssertionManager;
            assertionFactory = testFactory.AssertionFactory;

            subtreeExpectation = new SubtreeExpectation("Age Verifier Subtree", "should have been called");

            calledSubtrees = new List<string>();
            calledSubtrees.Add("salaryCheckerSubtree");

            fakeAssertion = An<CollectionAssertion>();
            assertionFactory.WhenToldTo(x => x.GetCollectionAssertion("should have been called")).Return(fakeAssertion);
            fakeAssertion.WhenToldTo(x => x.Assert<string>(calledSubtrees, "ageVerifierSubtree")).Return(true);
        };

        private Because of = () =>
        {
            result = manager.AssertSubtreeTestPassed(calledSubtrees, subtreeExpectation);
        };

        private It should_create_a_collection_assertion_for_the_expectation = () =>
        {
            assertionFactory.WasToldTo(x => x.GetCollectionAssertion("should have been called"));
        };

        private It should_call_assert_on_the_returned_assertion_with_the_correct_subtree_name = () =>
        {
            fakeAssertion.WasToldTo(x => x.Assert(calledSubtrees, "ageVerifierSubtree"));
        };

        private It should_return_the_assertion_result = () =>
        {
            result.Passed.ShouldBeTrue();
            result.Name.ShouldEqual("ageVerifierSubtree");
            result.CalledSubtrees.ShouldEqual(calledSubtrees);
            result.Assertion.ShouldEqual("should have been called");
        };
    }
}