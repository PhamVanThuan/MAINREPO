using System;
using System.Linq;
using SAHL.Tools.DecisionTree.TestRunner.Lib;
using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner
{
    public class VerboseConsoleOutput : ITestRunnerOutput
    {
        private TestSuiteSummary testSuiteSummary;
        public VerboseConsoleOutput()
        {
            testSuiteSummary = new TestSuiteSummary();
            testSuiteSummary.Name = "Overall";
        }

        public void ScenarioFinished(ScenarioResult scenarioResult)
        {
            PrintScenarioVerbose(scenarioResult);
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

        private void PrintScenarioVerbose(ScenarioResult scenarioResult)
        {
            WriteInColour(String.Format("{0}", Utilities.CapitaliseFirstLetter(scenarioResult.ScenarioName)), ConsoleColor.Cyan);
            Console.WriteLine();
            foreach (var result in scenarioResult.Results)
            {
                if (!result.Passed)
                {
                    WriteInColour(String.Format("  >> {0} - FAILED", result.FormatAssertionMessage()), ConsoleColor.Red);
                    if (result.TestException != null)
                    {
                        Console.WriteLine("     {0}", result.TestException.Message);
                    }
                    else
                    {
                        Console.WriteLine("     {0}", result.FormatOutputMessage());
                    }
                    Console.WriteLine();
                }
                else
                {
                    WriteInColour(String.Format("  >> {0}", result.FormatAssertionMessage()), ConsoleColor.Green);
                    Console.WriteLine();
                }
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
}
