using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input DLL (Managed Only)")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output Location")]
        public string Output { get; set; }
    }
}
