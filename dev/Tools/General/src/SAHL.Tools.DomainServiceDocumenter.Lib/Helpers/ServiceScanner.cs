using System.Collections.Generic;
using SAHL.Tools.DomainServiceDocumenter.Lib.AssociationScanners;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Helpers
{
    public class ServiceScanner
    {

        public AssemblyScanner Scanner { get; private set; }
        public string BusinessModelName { get; private set; }
        public string DomainChecksName { get; private set; }
        public string SharedName { get; private set; }
        public string ServerName { get; private set; }

        public List<ITypeScanner> EnumerationScanner { get; private set; }
        public List<ITypeScanner> DomainCheckScanner { get; private set; }
        public List<ITypeScanner> ModelScanner { get; private set; }
        public List<ITypeScanner> ModelPropertyScanner { get; private set; }
        public List<ITypeScanner> EventScanner { get; private set; }
        public List<ITypeScanner> CommandScanner { get; private set; }
        public List<ITypeScanner> RuleScanner { get; private set; }
        public List<ITypeScanner> CommandHandlerScanner { get; private set; }

        public List<IAssociationScanner> AssociationScanner { get; private set; }

        public List<ModelModel> Models { get; private set; }
        public List<EnumerationModel> Enumerations { get; private set; }
        public List<CommandModel> Commands { get; private set; }
        public List<EventModel> Events { get; private set; }
        public List<DomainCheckModel> Checks { get; private set; }
        public List<RuleModel> Rules { get; private set; }
        public List<QueryModel> Queries { get; private set; }
        private ScanVariables Variables { get; set; }

        public ServiceScanner(ScanVariables variables)
        {

            Variables = variables;
            InitialiseAssemblies(Variables.ServiceName, Variables.InputPath);
            Scanner = new AssemblyScanner();
            InitialiseModels();
            InitialiseTypeScanners();
            InitialiseAssociationScanners();
        }

        private void InitialiseTypeScanners()
        {
            EnumerationScanner = new List<ITypeScanner>() {new EnumerationTypeScanner(Enumerations)};
            DomainCheckScanner = new List<ITypeScanner>() {new DomainCheckTypeScanner(Checks)};
            ModelScanner = new List<ITypeScanner>() {new ModelTypeScanner(Models, Enumerations)};
            ModelPropertyScanner = new List<ITypeScanner>() { new ModelPropertyTypeScanner(Models, Enumerations) };
            EventScanner = new List<ITypeScanner>() { new EventTypeScanner(Events, Models, Enumerations) };
            CommandScanner = new List<ITypeScanner>() {new CommandTypeScanner(Commands, Checks, Models, Enumerations), new QueryTypeScanner()};
            RuleScanner = new List<ITypeScanner>() {new RuleTypeScanner(Rules, Models)};
            CommandHandlerScanner = new List<ITypeScanner>() {new CommandHandlerTypeScanner(Commands, Events, Rules)};
        }

        private void InitialiseAssociationScanners()
        {
            AssociationScanner = new List<IAssociationScanner>() { new EventAssociationScanner(Events, Commands), new RuleAssociationScanner(Rules, Commands), new CheckAssociationScanner(Checks, Commands) };
        }

        private void InitialiseAssemblies(string serviceName, string inputPath)
        {
            // find the shared and server assemblies
            BusinessModelName = string.Format("{0}\\SAHL.Core.BusinessModel.dll", inputPath);
            DomainChecksName = string.Format("{0}\\SAHL.DomainServiceChecks.dll", inputPath);
            SharedName = string.Format("{0}\\SAHL.Services.Interfaces.{1}Domain.dll", inputPath, serviceName);
            ServerName = string.Format("{0}\\SAHL.Services.{1}Domain.dll", inputPath, serviceName);
        }

        private void InitialiseModels()
        {
            // relect their types and get all commands, queries, events and rules
            Models = new List<ModelModel>();
            Enumerations = new List<EnumerationModel>();
            Commands = new List<CommandModel>();
            Events = new List<EventModel>();
            Checks = new List<DomainCheckModel>();
            Rules = new List<RuleModel>();
            Queries = new List<QueryModel>();
        }

        public void ScanModels()
        {
            Scanner.Scan(SharedName, ModelScanner);
            Scanner.Scan(SharedName, ModelPropertyScanner);
        }

        public void ScanEnumerators()
        {
            Scanner.Scan(BusinessModelName, EnumerationScanner);
        }

        public void ScanDomainChecks()
        {
            Scanner.Scan(DomainChecksName, DomainCheckScanner);
        }

        public void ScanEvents()
        {
            Scanner.Scan(SharedName, EventScanner);
        }

        public void ScanRules()
        {
            Scanner.Scan(ServerName, RuleScanner);
        }

        public void ScanCommands()
        {
            Scanner.Scan(SharedName, CommandScanner);
        }

        public void ScanCommandHandlers()
        {
            Scanner.Scan(ServerName, CommandHandlerScanner);
        }

        public void ScanAssociations()
        {
            Scanner.ScanForAssociations(AssociationScanner);
        }

    }

}