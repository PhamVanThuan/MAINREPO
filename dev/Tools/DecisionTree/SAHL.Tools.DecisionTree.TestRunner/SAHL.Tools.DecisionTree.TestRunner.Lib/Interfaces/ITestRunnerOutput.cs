namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces
{
    public interface ITestRunnerOutput
    {
        void ScenarioFinished(ScenarioResult scenarioResult);
        void ScenarioStarted(string name);
        void TestSuiteFinished(string name);
        void TestSuiteStarted(string name);
    }
}