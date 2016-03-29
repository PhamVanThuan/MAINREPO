using CommandLine;

namespace SAHL.Tools.DomainServiceDocumenter.Commands
{
    public class CommandLineArguments
    {
        [Option('s', "serviceName", Required = true, HelpText = "The domain service name.")]
        public string ServiceName { get; set; }

        [Option('i', "inputPath", Required = true, HelpText = "The input path to search.")]
        public string InputPath { get; set; }

        [Option('o', "outputPath", Required = true, HelpText = "The output path for the generated code.")]
        public string OutputPath { get; set; }

    }
}