using System.IO;
using System.Reflection;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using SAHL.Tools.DomainServiceDocumenter.Lib.Templates;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Helpers
{
    public class HtmlOutputGenerator
    {

        private ScanDirectoryUtility ScanDirectoryUtility { get; set; }
        private ServiceModel ServiceModel { get; set; }
        private ServiceScanner ServiceScanner{ get; set; }
        private string OutputPath { get; set; }

        public HtmlOutputGenerator(string serviceName, string outputPath, ScanDirectoryUtility scanDirectoryUtility, ServiceScanner serviceScanner)
        {
            ScanDirectoryUtility = scanDirectoryUtility;
            ServiceModel = new ServiceModel(serviceName, "0.0.0.0");
            OutputPath = outputPath;
            ServiceScanner = serviceScanner;
        }

        public void WriteCSSFile()
        {
            // copy the css directory
            string cssMin = Path.Combine(ScanDirectoryUtility.CssPath, "bootstrap.min.css");
            WriteResourceToFile("SAHL.Tools.DomainServiceDocumenter.Lib.Styles.css.bootstrap.min.css", cssMin);
        }

        public void WriteIndex()
        {
            Index idx = new Index(ServiceModel);
            string indexHtml = idx.TransformText();
            string outputFile = string.Format("{0}\\index.html", OutputPath);
            WriteFile(outputFile, indexHtml);
        }

        public void WriteCommands()
        {
            // generate the commands
            Commands cmds = new Commands(ServiceScanner.Commands, ServiceModel);
            string commandsHtml = cmds.TransformText();
            string outputFile = string.Format("{0}\\commands.html", OutputPath);
            WriteFile(outputFile, commandsHtml);

            // do the commands page
            foreach (var cmd in ServiceScanner.Commands)
            {
                // do each individual command
                Command command = new Command(ServiceScanner.Commands, cmd, ServiceModel);
                string commandHtml = command.TransformText();
                outputFile = string.Format("{0}\\commands\\{1}.html", OutputPath, cmd.Name.ToLower());
                WriteFile(outputFile, commandHtml);
            }
        }

        public void WriteQueries()
        {

            // generate the queries
            Queries qrys = new Queries(ServiceScanner.Queries, ServiceModel);
            string queriesHtml = qrys.TransformText();
            string outputFile = string.Format("{0}\\queries.html", OutputPath);
            WriteFile(outputFile, queriesHtml);

            // do the queries page
            foreach (var qry in ServiceScanner.Queries)
            {
                // do each individual command
                Query query = new Query(ServiceScanner.Queries, qry, ServiceModel);
                string queryHtml = query.TransformText();
                outputFile = string.Format("{0}\\queries\\{1}.html", OutputPath, qry.Name.ToLower());
                WriteFile(outputFile, queryHtml);
            }
        }

        public void WriteEvents()
        {
            // generate the events
            Events evts = new Events(ServiceScanner.Events, ServiceModel);
            string eventsHtml = evts.TransformText();
            string outputFile = string.Format("{0}\\events.html", OutputPath);
            WriteFile(outputFile, eventsHtml);

            // do the events page
            foreach (var evt in ServiceScanner.Events)
            {
                // do each individual event
                Event @event = new Event(ServiceScanner.Events, evt, ServiceModel);
                string eventHtml = @event.TransformText();
                outputFile = string.Format("{0}\\events\\{1}.html", OutputPath, evt.Name.ToLower());
                WriteFile(outputFile, eventHtml);
            }
        }

        public void WriteModels()
        {

            // generate the models
            Templates.Models mdls = new Templates.Models(ServiceScanner.Models, ServiceModel);
            string modelsHtml = mdls.TransformText();
            string outputFile = string.Format("{0}\\models.html", OutputPath);
            WriteFile(outputFile, modelsHtml);

            // do the models page
            foreach (var mdl in ServiceScanner.Models)
            {
                // do each individual model
                Model model = new Model(ServiceScanner.Models, mdl, ServiceModel);
                string modelHtml = model.TransformText();
                outputFile = string.Format("{0}\\models\\{1}.html", OutputPath, mdl.Name.ToLower());
                WriteFile(outputFile, modelHtml);
            }

        }


        public void WriteEmerators()
        {
            // generate the commands
            Enumerations enumerations = new Enumerations(ServiceScanner.Enumerations, ServiceModel);
            string enumerationsHtml = enumerations.TransformText();
            string outputFile = string.Format("{0}\\enumerators.html", OutputPath);
            WriteFile(outputFile, enumerationsHtml);

            // do the commands page
            foreach (var enumeration in ServiceScanner.Enumerations)
            {
                // do each individual command
                Enumeration enumeration1 = new Enumeration(ServiceScanner.Enumerations, enumeration, ServiceModel);
                string commandHtml = enumeration1.TransformText();
                outputFile = string.Format("{0}\\enumerators\\{1}.html", OutputPath, enumeration.Name.ToLower());
                WriteFile(outputFile, commandHtml);
            }
        }


        public void WriteRules()
        {
            // generate the rules
            Templates.Rules ruls = new Templates.Rules(ServiceScanner.Rules, ServiceModel);
            string rulesHtml = ruls.TransformText();
            string outputFile = string.Format("{0}\\rules.html", OutputPath);
            WriteFile(outputFile, rulesHtml);

            // do the models page
            foreach (var rul in ServiceScanner.Rules)
            {
                // do each individual model
                Rule model = new Rule(ServiceScanner.Rules, rul, ServiceModel);
                string ruleHtml = model.TransformText();
                outputFile = string.Format("{0}\\rules\\{1}.html", OutputPath, rul.Name.ToLower());
                WriteFile(outputFile, ruleHtml);
            }
        }

        public void WriteDomainChecks()
        {
            // generate the domain checks
            Templates.DomainChecks domainChecks = new Templates.DomainChecks(ServiceScanner.Checks, ServiceModel);
            string domainChecksHtml = domainChecks.TransformText();
            string outputFile = string.Format("{0}\\domainchecks.html", OutputPath);
            WriteFile(outputFile, domainChecksHtml);

            // do the models page
            foreach (var chk in ServiceScanner.Checks)
            {
                // do each individual model
                DomainCheck domainCheck = new DomainCheck(ServiceScanner.Checks, chk, ServiceModel);
                string chkHtml = domainCheck.TransformText();
                outputFile = string.Format("{0}\\domainchecks\\{1}.html", OutputPath, chk.Name.ToLower());
                WriteFile(outputFile, chkHtml);
            }
        }

        public void WriteResourceToFile(string resourceName, string fileName)
        {
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        private void WriteFile(string outputFile, string commandsHtml)
        {
            using (var SW = new StreamWriter(outputFile))
            {
                SW.Write(commandsHtml);
                SW.Flush();
            }
        }

        
    }

}