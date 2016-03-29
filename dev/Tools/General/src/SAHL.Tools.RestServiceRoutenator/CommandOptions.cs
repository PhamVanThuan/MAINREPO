using CommandLine;

namespace SAHL.Tools.RestServiceRoutenator
{
    public class CommandOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input DLL (Managed Only)")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output Location")]
        public string Output { get; set; }

        [Option('n', "namespace", Required = true, HelpText = "Default Namespace")]
        public string Namespace { get; set; }

        [Option('p', "prefix", Required = false, HelpText = "Name Prefix", DefaultValue = "")]
        public string Prefix { get; set; }

        [Option('l', "libFolder", Required = false, HelpText = "Name Prefix", DefaultValue = "app")]
        public string libFolder { get; set; }
    }
}