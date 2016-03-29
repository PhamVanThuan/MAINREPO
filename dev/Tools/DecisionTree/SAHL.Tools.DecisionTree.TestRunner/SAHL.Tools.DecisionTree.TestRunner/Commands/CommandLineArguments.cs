using CommandLine;

namespace SAHL.Tools.DecisionTree.TestRunner.Commands
{
    public class CommandLineArguments
    {
        [Option('t', "treeAssembly", Required = true, HelpText = "The path to the Assembly containing the Decision Trees to run tests against.")]
        public string TreeAssembly { get; set; }

        [Option('a', "testAssembly", Required = true, HelpText = "The path to the Assembly containing the Decision Tree tests to run.")]
        public string TestAssembly { get; set; }

        [Option('n', "testName", Required = false, HelpText = "The name of a singular test that will be run.")]
        public string TestName { get; set; }

        [Option('v', "version", Required = false, HelpText = "The version of a singular test that will be run.", DefaultValue = 1)]
        public int Version { get; set; }

        [Option('c', "console", Required = false, HelpText = "Send all output to console (not TeamCity).", DefaultValue = false)]
        public bool Console { get; set; }

        [Option('r', "reportCoverage", Required = false, HelpText = "Generate a coverage report for the decision tree tests.", DefaultValue = false)]
        public bool ReportCoverage { get; set; }

        [Option('o', "reportOutputPath", Required =false, HelpText ="Output path for generated coverage reports.", DefaultValue = ".\\CoverageReport")]
        public string ReportOutputPath { get;set; }
    }
}
