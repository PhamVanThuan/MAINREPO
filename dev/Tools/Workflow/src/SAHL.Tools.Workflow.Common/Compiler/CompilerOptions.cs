using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Compiler
{
    public class CompilerOptions
    {
        public CompilerOptions(string outputDirectory, string binariesDirectory, bool reloadReferences)
        {
            this.OutputDirectory = outputDirectory;
            this.BinariesDirectory = binariesDirectory;
            this.ReloadReferences = reloadReferences;
        }
        public string OutputDirectory { get; protected set; }
		public string BinariesDirectory { get; protected set; }
        public bool ReloadReferences { get; protected set; }
    }
}