using System;
using System.Linq;
using SAHL.Tools.DecisionTree.TestRunner.Lib;
using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner
{
    public class ConsoleOutput : ITestRunnerOutput
    {
        private TestSuiteSummary testSuiteSummary;
        public ConsoleOutput()
        {
            testSuiteSummary = new TestSuiteSummary();
            testSuiteSummary.Name = "Overall";
        }

        public void ScenarioFinished(ScenarioResult scenarioResult)
        {
            PrintScenarioProgress(scenarioResult.Passed());
        }

        public void ScenarioStarted(string name)
        {
        }

        public void TestSuiteFinished(string name)
        {
            Console.WriteLine();
        }

        public void TestSuiteStarted(string name)
        {
            WriteInColour(String.Format("Started {0}", name), ConsoleColor.Yellow);
        }

        private void PrintScenarioProgress(bool passed)
        {
            if (!passed)
            {
                WriteInColour("X", ConsoleColor.Red, false);
            }
            else
            {
                WriteInColour(".", ConsoleColor.Green, false);
            }
        }

        private void PrintSummary(int totalTests, int totalFailed)
        {
            Console.WriteLine();
            Console.WriteLine("Scenarios run : {0}", totalTests);
            Console.WriteLine("Passed : {0}", totalTests - totalFailed);
            Console.WriteLine("Failed : {0}", totalFailed);
            Console.WriteLine();
            if (totalTests == 0)
            {
                WriteInColour("NO TESTS", ConsoleColor.Yellow);
            }
            else if (totalFailed > 0)
            {
                WriteInColour("FAILED", ConsoleColor.Red);
            }
            else
            {
                WriteInColour("PASSED", ConsoleColor.Green);
            }
        }

        private void WriteInColour(string message, ConsoleColor colour, bool writeline = true, ConsoleColor originalColour = ConsoleColor.White)
        {
            Console.ForegroundColor = colour;
            if (writeline)
                Console.WriteLine(message);
            else
                Console.Write(message);
            Console.ForegroundColor = originalColour;
        }
    }

    public class TestSuiteSummary
    {
        public string Name { get; set; }
        public int TestsRun { get; set; }
        public int TestsFailed { get; set; }
        public List<TestSuiteSummary> ChildTestSuites { get; set; }

        public TestSuiteSummary()
        {
            ChildTestSuites = new List<TestSuiteSummary>();
            TestsRun = 0;
            TestsFailed = 0;
        }
    }
}
