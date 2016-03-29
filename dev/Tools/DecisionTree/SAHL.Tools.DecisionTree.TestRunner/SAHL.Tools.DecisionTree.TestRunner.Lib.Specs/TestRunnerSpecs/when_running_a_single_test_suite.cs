using Machine.Fakes;
using Machine.Specifications;

using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs
{
    public class when_running_a_single_test_suite : WithFakes
    {
        private static ITestEngine testEngine;
        private static TestRunner testRunner;
        private static ITestSuite testSuite;

        private static List<string> testSuiteStartedEvents;
        private static List<string> scenarioStartedEvents;
        private static List<ScenarioResult> scenarioFinishedEvents;
        private static List<string> testSuiteFinishedEvents;

        private static string treeName;
        private static int treeVersion;
        private static ScenarioResult result;

        private Establish context = () =>
            {
                testEngine = An<ITestEngine>();
                testSuite = new FakeDecisionTree_2TestSuite();
                testRunner = new TestRunner(testEngine);
                treeName = "FakeDecisionTree";
                treeVersion = 2;

                result = new ScenarioResult("given water", "FakeDecisionTree v2");

                testSuiteStartedEvents = new List<string>();
                scenarioStartedEvents = new List<string>();
                scenarioFinishedEvents = new List<ScenarioResult>();
                testSuiteFinishedEvents = new List<string>();

                testRunner.TestSuiteStarted += (caller, name) => { testSuiteStartedEvents.Add(name); };
                testRunner.ScenarioStarted += (caller, name) => { scenarioStartedEvents.Add(name); };
                testRunner.ScenarioFinished += (caller, scenarioResult) => { scenarioFinishedEvents.Add(scenarioResult); };
                testRunner.TestSuiteFinished += (caller, name) => { testSuiteFinishedEvents.Add(name); };

                testEngine.WhenToldTo(x => x.ExecuteScenario(testSuite.TestCases.First().TestCaseScenarios.First(), Param.IsAny<List<ITestInput>>(), Param.IsAny<string>())).Return(result);
            };

        private Because of = () =>
            {
                testRunner.RunTestSuite(testSuite);
            };

        private It should_set_up_the_test_suite_in_the_engine = () =>
            {
                testEngine.WasToldTo(x => x.SetupTestSuite(treeName, treeVersion));
            };

        private It should_broadcast_the_test_suite_started_event_with_the_tree_name = () =>
            {
                testSuiteStartedEvents[0].ShouldEqual("FakeDecisionTree v2");
            };

        private It should_broadcast_the_test_suite_started_event_with_the_test_case_name = () =>
            {
                testSuiteStartedEvents[1].ShouldEqual("when planting a tree");
            };

        private It should_broadcast_the_scenario_started_event_with_the_scenario_name = () =>
            {
                scenarioStartedEvents[0].ShouldEqual("given water");
            };

        private It should_call_the_test_enigne_to_execute_the_scenario = () =>
            {
                testEngine.WasToldTo(x => x.ExecuteScenario(testSuite.TestCases.First().TestCaseScenarios.First(),
                    Param<List<ITestInput>>.Matches(m => m.Intersect(testSuite.TestCases.First().TestCaseInputs).Any()),
                    Param.IsAny<string>()));
            };

        private It should_broadcast_the_scenario_result = () =>
            {
                scenarioFinishedEvents.First().ShouldEqual(result);
            };

        private It should_broadcast_the_test_case_has_ended = () =>
            {
                testSuiteFinishedEvents[0].ShouldEqual("when planting a tree");
            };

        private It should_broadcast_the_test_suite_has_ended = () =>
        {
            testSuiteFinishedEvents[1].ShouldEqual("FakeDecisionTree v2");
        };
    }

    public class FakeDecisionTree_2TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; private set; }

        public FakeDecisionTree_2TestSuite()
        {
            this.TestCases = new List<ITestCase>
            {
                new TestCase("when planting a tree", new List<ITestInput>
                {
                    new TestInput("seed", "string", "walnut")
                }, new List<IScenario>
                {
                    new Scenario("given water", new List<ITestInput>(), new List<ITestOutput>(), new List<IOutputMessage>(), new List<ISubtreeExpectation>())
                })
            };
        }
    }
}