namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public delegate void ScenarioStartedEventHandler(object sender, string name);
    public delegate void ScenarioFinishedEventHandler(object sender, ScenarioResult scenarioResult);
    public delegate void TestSuiteStartedEventHandler(object sender, string name);
    public delegate void TestSuiteFinishedEventHandler(object sender, string name);

}
