using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;

using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.TestEngineSpecs
{
    public class when_executing_a_scenario : WithFakes
    {
        private static TestEngine testEngine;
        private static ITreeExecutionManager treeExecutionManager;
        private static ITestProcessor testProcessor;
        private static IScenario scenario;
        private static List<ITestInput> testCaseInputs;
        private static string treeAssemblyPath;
        private static int treeVersion;
        private static string treeName;
        private static ITestResult outputResult;
        private static ITestResult messageResult;
        private static ExpandoObject treeVariablesObject;
        private static ExpandoObject enumerations;
        private static ScenarioResult result;

        private Establish context = () =>
        {
            treeExecutionManager = An<ITreeExecutionManager>();
            testProcessor = An<ITestProcessor>();
            testEngine = new TestEngine(testProcessor, treeExecutionManager);

            treeAssemblyPath = "C:\\Path\\to\\Decision\\Tree.dll";
            treeName = "Awesome name formatter tree";
            treeVersion = 2;
            testEngine.SetupTestSuite(treeName, treeVersion);
            testCaseInputs = new List<ITestInput>();
            var scenarioInputs = new List<ITestInput> { new TestInput("Name", "string", "John Doe") };
            var scenarioOutputs = new List<ITestOutput> { new TestOutput("FormattedName", "to equal", "string", "Lord John Doe") };
            var expectedMessages = new List<IOutputMessage> { new OutputMessage("Formatting name...", "to contain", "Info") };
            scenario = new Scenario("When testing a tree", scenarioInputs, scenarioOutputs, expectedMessages, new List<ISubtreeExpectation>());

            outputResult = new TestOutputResult(scenarioOutputs[0], "Lord John Doe", "Lord John Doe", true);
            messageResult = new TestMessageResult(expectedMessages[0], new List<ISystemMessage>(), false);

            treeVariablesObject = new ExpandoObject();
            enumerations = new ExpandoObject();
            treeExecutionManager.WhenToldTo(x => x.TreeVariablesObject).Return(treeVariablesObject);
            treeExecutionManager.WhenToldTo(x => x.Enumerations).Return(enumerations);
            testProcessor.WhenToldTo(x => x.ProcessDecisionTreeOutputResults(Param.IsAny<object>(), Param.IsAny<List<ITestOutput>>(), Param.IsAny<object>())).Return(new List<ITestResult> { outputResult });
            testProcessor.WhenToldTo(x => x.ProcessDecisionTreeMessageResults(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<List<IOutputMessage>>())).Return(new List<ITestResult> { messageResult });
        };

        private Because of = () =>
            {
                result = testEngine.ExecuteScenario(scenario, testCaseInputs, treeAssemblyPath);
            };

        private It should_set_up_the_tree_execution_manager = () =>
        {
            treeExecutionManager.WasToldTo(x => x.SetupTreeExecutionManager(treeAssemblyPath, Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_process_the_tree_through_the_tree_execution_manager = () =>
            {
                treeExecutionManager.WasToldTo(x => x.Process(treeName, treeVersion, testCaseInputs,
                    Param<List<ITestInput>>.Matches(m => m.Intersect(scenario.ScenarioInputs).Any())));
            };

        private It should_process_the_test_outputs = () =>
            {
                testProcessor.WasToldTo(x => x.ProcessDecisionTreeOutputResults(treeVariablesObject, Param<List<ITestOutput>>.Matches(m => m.Intersect(scenario.ExpectedOutputs).Any()), enumerations));
            };

        private It should_process_the_decision_tree_messages = () =>
            {
                testProcessor.WasToldTo(x => x.ProcessDecisionTreeMessageResults(Param.IsAny<ISystemMessageCollection>(), Param<List<IOutputMessage>>.Matches(m => m.Intersect(scenario.ExpectedMessages).Any())));
            };

        private It should_return_the_result = () =>
            {
                result.DecisionTreeName.Contains(treeName).ShouldBeTrue();
                result.DecisionTreeName.Contains(treeVersion.ToString()).ShouldBeTrue();
                result.Results.ShouldContain(messageResult);
                result.Results.ShouldContain(outputResult);
                result.ScenarioName.ShouldEqual(scenario.Name);
            };
    }
}