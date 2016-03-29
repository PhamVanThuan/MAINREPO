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
    public class when_executing_a_scenario_given_subtrees : WithFakes
    {
        private static TestEngine testEngine;
        private static ITreeExecutionManager treeExecutionManager;
        private static ITestProcessor testProcessor;
        private static IScenario scenario;
        private static List<ITestInput> testCaseInputs;
        private static string treeAssemblyPath;
        private static int treeVersion;
        private static string treeName;
        private static ITestResult subtreeResult;
        private static ScenarioResult result;

        private static List<string> calledSubtrees;

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
            var subtreeExpectations = new List<ISubtreeExpectation> { new SubtreeExpectation("Walnut", "should have been called") };
            scenario = new Scenario("When testing a tree", new List<ITestInput>(), new List<ITestOutput>(), new List<IOutputMessage>(), subtreeExpectations);

            calledSubtrees = new List<string> { "Walnut", "Oak" };
            subtreeResult = new SubtreeExpectationResult("Walnut", "should have been called", calledSubtrees, true);

            var treeVariables = new FakeDecisionTreeVariables();
            treeVariables.subtrees = new Dictionary<string, dynamic>();
            treeVariables.subtrees.Add("Walnut", new object());
            treeVariables.subtrees.Add("Oak", new object());
            var treeVariablesObject = Utilities.ToDynamic(treeVariables);
            var enumerations = new ExpandoObject();
            treeExecutionManager.WhenToldTo(x => x.TreeVariablesObject).Return(treeVariablesObject);
            treeExecutionManager.WhenToldTo(x => x.Enumerations).Return(enumerations);
            testProcessor.WhenToldTo(x => x.ProcessDecisionTreeOutputResults(Param.IsAny<object>(), Param.IsAny<List<ITestOutput>>(), Param.IsAny<object>())).Return(new List<ITestResult>());
            testProcessor.WhenToldTo(x => x.ProcessDecisionTreeMessageResults(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<List<IOutputMessage>>())).Return(new List<ITestResult>());
            testProcessor.WhenToldTo(x => x.ProcessDecisionTreeSubtreeResults(treeVariables.subtrees, Param<List<ISubtreeExpectation>>.Matches(m => m.Intersect(scenario.SubtreeExpectations).Any()))).Return(new List<ITestResult> { subtreeResult });
        };

        private Because of = () =>
            {
                result = testEngine.ExecuteScenario(scenario, testCaseInputs, treeAssemblyPath);
            };

        private It should_process_the_subtree_results = () =>
            {
                testProcessor.WasToldTo(x => x.ProcessDecisionTreeSubtreeResults(
                    Param<IDictionary<string, dynamic>>.Matches(m => m.Keys.Intersect(calledSubtrees).Any()),
                    Param<List<ISubtreeExpectation>>.Matches(m => m.Intersect(scenario.SubtreeExpectations).Any())));
            };

        private It should_return_the_result = () =>
            {
                result.DecisionTreeName.Contains(treeName).ShouldBeTrue();
                result.DecisionTreeName.Contains(treeVersion.ToString()).ShouldBeTrue();
                result.Results.ShouldContain(subtreeResult);
                result.ScenarioName.ShouldEqual(scenario.Name);
            };
    }

    public class FakeDecisionTreeVariables
    {
        public IDictionary<string, dynamic> subtrees { get; set; }
    }
}