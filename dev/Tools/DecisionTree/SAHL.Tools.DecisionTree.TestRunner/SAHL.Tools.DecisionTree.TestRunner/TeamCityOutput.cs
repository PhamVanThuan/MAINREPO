using System;
using SAHL.Tools.DecisionTree.TestRunner.Lib;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner
{
    public class TeamCityOutput : ITestRunnerOutput
    {
        private string testSuiteStartedMessage = "testSuiteStarted name='{0}'";
        private string testSuiteFinishedMessage = "testSuiteFinished name='{0}'";
        private string testStartedMessage = "testStarted name='{0}'";
        private string testFinishedMessage = "testFinished name='{0}'";

        public void TestSuiteStarted(string name)
        {
            PrintTeamCityServiceMessage(String.Format(testSuiteStartedMessage, StripSpaces(name)));
        }
        public void TestSuiteFinished(string name)
        {
            PrintTeamCityServiceMessage(String.Format(testSuiteFinishedMessage, StripSpaces(name)));
        }

        public void ScenarioStarted(string name)
        {
            PrintTeamCityServiceMessage(String.Format(testSuiteStartedMessage, StripSpaces(name)));
        }

        public void ScenarioFinished(ScenarioResult scenarioResult)
        {
            foreach (var result in scenarioResult.Results)
            {
                string testName = String.Format("{0}.{1}.{2}", scenarioResult.DecisionTreeName, scenarioResult.ScenarioName, result.FormatAssertionMessage());
                testName = StripSpaces(testName);
                PrintTeamCityServiceMessage(String.Format(testStartedMessage, result.FormatAssertionMessage()));
                if (!result.Passed)
                {
                    if (result.TestException != null)
                    {
                        string exceptionMessage = String.Format("testStdErr name='{0}' out='{1}'", testName, result.FormatExceptionMessage());
                        PrintTeamCityServiceMessage(exceptionMessage);
                        PrintTeamCityServiceMessage(String.Format("testFailed name='{0}' message='{1}' details='An Error Occurred : {2}'", testName, result.FormatAssertionMessage(), result.TestException.Message));
                    }
                    else
                    {
                        PrintTeamCityServiceMessage(String.Format("testFailed name='{0}' message='{1}' details='{2}'",
                            testName, result.FormatAssertionMessage(), result.FormatOutputMessage()));
                    }
                }
                PrintTeamCityServiceMessage(String.Format(testFinishedMessage, result.FormatAssertionMessage()));
            }
            PrintTeamCityServiceMessage(String.Format(testSuiteFinishedMessage, StripSpaces(scenarioResult.ScenarioName)));
        }

        private void PrintTeamCityServiceMessage(string message)
        {
            Console.WriteLine("##teamcity[{0}]", message);
        }

        private string StripSpaces(string original)
        {
            return original.Replace(" ", "_");
        }
    }
}
