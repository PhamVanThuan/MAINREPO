using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs
{
    public class when_processing_called_subtree_results_given_a_was_called_assertion : WithFakes
    {
        private static TestProcessorFactory factory;
        private static TestProcessor processor;
        private static List<ISubtreeExpectation> subtreeExpectations;
        private static IDictionary<string, dynamic> calledSubtrees;
        private static List<ITestResult> result;

        private Establish context = () =>
        {
            factory = new TestProcessorFactory();
            processor = factory.Processor;

            subtreeExpectations = new List<ISubtreeExpectation>
            {
                new SubtreeExpectation("Age Verifier Subtree", "should have been called"),
                new SubtreeExpectation("Salary Checker Subtree", "should have been called")
            };

            calledSubtrees = new Dictionary<string, dynamic>();
            calledSubtrees.Add("salaryCheckerSubtree", new object());
        };

        private Because of = () =>
        {
            result = processor.ProcessDecisionTreeSubtreeResults(calledSubtrees, subtreeExpectations);
        };

        private It should_call_the_assertion_manager_for_the_first_subtree = () =>
        {
            factory.assertionManager.WasToldTo(x => x.AssertSubtreeTestPassed(Param<List<string>>.Matches(y => y.Contains("salaryCheckerSubtree")), subtreeExpectations[0]));
        };

        private It should_call_the_assertion_manager_for_the_second_subtree = () =>
        {
            factory.assertionManager.WasToldTo(x => x.AssertSubtreeTestPassed(Param<List<string>>.Matches(y => y.Contains("salaryCheckerSubtree")), subtreeExpectations[1]));
        };

        private It should_return_a_result_for_each_expectation = () =>
        {
            result.Count.ShouldEqual(2);
        };
    }
}