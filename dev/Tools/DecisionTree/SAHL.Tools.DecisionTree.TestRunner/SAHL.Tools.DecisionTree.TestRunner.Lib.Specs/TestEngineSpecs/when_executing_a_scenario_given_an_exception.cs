using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.TestEngineSpecs
{
    public class when_executing_a_scenario_given_an_exception : WithFakes
    {
        private static TestEngine testEngine;
        private static IScenario scenario;
        private static List<ITestInput> testCaseInputs;
        private static string treeAssemblyPath;
        private static ScenarioResult result;
        private static Exception thrownException;

        private Establish context = () =>
        {
            var treeExecutionManager = An<ITreeExecutionManager>();
            var testProcessor = An<ITestProcessor>();
            testEngine = new TestEngine(testProcessor, treeExecutionManager);

            treeAssemblyPath = "C:\\Path\\to\\Decision\\Tree.dll";
            var treeName = "Awesome name formatter tree";
            var treeVersion = 2;
            testEngine.SetupTestSuite(treeName, treeVersion);
            testCaseInputs = new List<ITestInput>();
            var scenarioInputs = new List<ITestInput> { new TestInput("Name", "string", "John Doe") };
            var scenarioOutputs = new List<ITestOutput> { new TestOutput("FormattedName", "to equal", "string", "Lord John Doe") };
            var expectedMessages = new List<IOutputMessage> { new OutputMessage("Formatting name...", "to contain", "Info") };
            var expectedSubtrees = new List<ISubtreeExpectation> { new SubtreeExpectation("Walnut", "should have been called") };
            scenario = new Scenario("When testing a tree", scenarioInputs, scenarioOutputs, expectedMessages, expectedSubtrees);

            treeExecutionManager.WhenToldTo(x => x.Process(Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<List<ITestInput>>(), Param.IsAny<List<ITestInput>>())).Throw(new Exception());
        };

        private Because of = () =>
            {
                thrownException = Catch.Exception(() => result = testEngine.ExecuteScenario(scenario, testCaseInputs, treeAssemblyPath));
            };

        private It should_catch_the_exception = () =>
            {
                thrownException.ShouldBeNull();
            };

        private It should_create_an_error_result_for_the_subtree_expectation = () =>
            {
                result.Results.ShouldContain(m => m.TestException != null && m.Name == "Walnut" && m.Passed == false);
            };

        private It should_create_an_error_result_for_the_output_expectation = () =>
            {
                result.Results.ShouldContain(m => m.TestException != null && m.Name == scenario.ExpectedOutputs.First().Name && m.Passed == false);
            };

        private It should_create_an_error_result_for_the_messages_expectation = () =>
            {
                result.Results.ShouldContain(m => m.TestException != null && m.Name == scenario.ExpectedMessages.First().ExpectedMessageSeverity && m.Passed == false);
            };

        private It should_return_the_result = () =>
            {
                result.ShouldNotBeNull();
            };
    }
}