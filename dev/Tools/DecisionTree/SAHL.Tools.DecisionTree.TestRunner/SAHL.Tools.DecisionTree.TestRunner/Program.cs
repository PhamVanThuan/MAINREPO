using System;
using CommandLine;
using SAHL.Tools.DecisionTree.TestRunner.Commands;
using SAHL.Tools.DecisionTree.TestRunner.Lib;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using StructureMap;

namespace SAHL.Tools.DecisionTree.TestRunner
{
    class Program
    {
        private static IContainer container;
        private static ITestRunner testRunner;
        private static ITestRunnerOutput testRunnerOutput;
        private static ICoverageMonitor coverage;

        static void Main(string[] args)
        {
            var commands = new CommandLineArguments();
            var parser = new Parser();
            if (parser.ParseArguments(args, commands))
            {
                container = IoC.Initialize();
                testRunner = container.GetInstance<ITestRunner>();

                string testName = String.IsNullOrEmpty(commands.TestName) ? String.Empty : commands.TestName.Trim();
                int testVersion = commands.Version;

                SetupCoverageReport(commands);
                SetTestRunnerOutput(commands, testName);           
                SetupTestRunnerEvents();

                if (String.IsNullOrEmpty(testName))
                {
                    testRunner.RunAllTestSuitesInAssembly(commands.TreeAssembly, commands.TestAssembly);
                }
                else
                {
                    testRunner.RunTestsForDecisionTree(testName, testVersion, commands.TreeAssembly, commands.TestAssembly);
                }
                WriteCoverageReport(commands);
                
                if (commands.Console)
                {
                    Console.Read();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error parsing command line arguments.");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(-1);
            }
        }

        private static void SetupCoverageReport(CommandLineArguments commands)
        {
            if (commands.ReportCoverage)
            {
                coverage = container.GetInstance<ICoverageMonitor>();
                coverage.BindToTreeExecutionManager(container.GetInstance<ITreeExecutionManager>());
            }
        }

        private static void SetTestRunnerOutput(CommandLineArguments commands, string testName)
        {
            // User has passed "-c true" or "--console=true"
            if (commands.Console)
            {
                if (!String.IsNullOrEmpty(testName))
                {
                    testRunnerOutput = new VerboseConsoleOutput();
                }
                else
                {
                    testRunnerOutput = new ConsoleOutput();
                }
            }
            else
            {
                testRunnerOutput = new TeamCityOutput();
            }
        }

        private static void SetupTestRunnerEvents()
        {
            testRunner.ScenarioStarted += TestRunner_ScenarioStarted;
            testRunner.ScenarioFinished += TestRunner_ScenarioFinished;
            testRunner.TestSuiteFinished += TestRunner_TestSuiteFinished;
            testRunner.TestSuiteStarted += TestRunner_TestSuiteStarted;
        }

        private static void WriteCoverageReport(CommandLineArguments commands)
        {
            if (commands.ReportCoverage)
            {
                coverage.WriteCoverageResult(commands.ReportOutputPath, container.GetInstance<ICoverageResultWriter>("HTML"));
                coverage.WriteCoverageResult(commands.ReportOutputPath, container.GetInstance<ICoverageResultWriter>("XML"));
            }
        }


        private static void TestRunner_ScenarioStarted(object sender, string name)
        {
            testRunnerOutput.ScenarioStarted(name);
        }

        private static void TestRunner_TestSuiteStarted(object sender, string name)
        {
            testRunnerOutput.TestSuiteStarted(name);
        }
        private static void TestRunner_TestSuiteFinished(object sender, string name)
        {
            testRunnerOutput.TestSuiteFinished(name);
        }
        private static void TestRunner_ScenarioFinished(object sender, ScenarioResult scenarioResult)
        {
            testRunnerOutput.ScenarioFinished(scenarioResult);
        }
    }
}
