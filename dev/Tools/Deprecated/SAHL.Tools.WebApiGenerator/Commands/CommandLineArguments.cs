using CommandLine;

namespace SAHL_Tools.WebApiGenerator.Commands
{
    public class CommandLineArguments
    {
        [Option('i', "inputPath", Required = true, HelpText = "The path to the file containing the service interface.")]
        public string InputFile { get; set; }

        [Option('o', "outptPath", Required = true, HelpText = "The path to the file to generate.")]
        public string OutputFile { get; set; }
    }
}
