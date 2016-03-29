using System.IO;
using SAHL.Tools.DomainServiceDocumenter.Lib.Templates;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Helpers
{
    public class ScanDirectoryUtility
    {

        public string CssPath { get; private set; }
        public string EventsPath { get; private set; }
        public string CommandsPath { get; private set; }
        public string ModelsPath { get; private set; }
        public string EnumeratorsPath { get; private set; }
        public string RulesPath { get; private set; }
        public string DomainChecksPath { get; private set; }
        private ScanVariables Variables { get; set; }

        public ScanDirectoryUtility(ScanVariables variables)
        {
            Variables = variables;
        }

        public void PrepareWorkingDirectories()
        {
            // check the output folder exists
            CreateDirectory(Variables.OutputPath);
            CreateWorkingPaths(Variables.OutputPath);
            CreateWorkingDirectories();
        }

        private void CreateWorkingDirectories()
        {
            CreateDirectory(CssPath);
            CreateDirectory(CommandsPath);
            CreateDirectory(EventsPath);
            CreateDirectory(ModelsPath);
            CreateDirectory(EnumeratorsPath);
            CreateDirectory(RulesPath);
            CreateDirectory(DomainChecksPath);
        }

        private void CreateWorkingPaths(string outputPath)
        {
            CssPath = Path.Combine(outputPath, "css");
            CommandsPath = Path.Combine(outputPath, "commands");
            EventsPath = Path.Combine(outputPath, "events");
            ModelsPath = Path.Combine(outputPath, "models");
            EnumeratorsPath = Path.Combine(outputPath, "enumerators");
            RulesPath = Path.Combine(outputPath, "rules");
            DomainChecksPath = Path.Combine(outputPath, "domainchecks");
        }

        private void CreateDirectory(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
        }
    }
}