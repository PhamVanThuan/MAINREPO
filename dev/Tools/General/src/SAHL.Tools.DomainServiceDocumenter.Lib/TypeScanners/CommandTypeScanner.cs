using Mono.Cecil;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;
using System.Linq;
using SAHL.Tools.DomainServiceDocumenter.Lib.Templates;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class CommandTypeScanner : ITypeScanner
    {
        private List<CommandModel> commands;
        private List<DomainCheckModel> checks;
        private List<ModelModel> models;
        private List<EnumerationModel> enumerations;

        public CommandTypeScanner(List<CommandModel> commands, List<DomainCheckModel> checks, List<ModelModel> models, List<EnumerationModel> enumerations)
        {
            this.commands = commands;
            this.checks = checks;
            this.models = models;
            this.enumerations = enumerations;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == false && typeToProcess.Name.EndsWith("Command") && !typeToProcess.Interfaces.Any(x=>x.Name.EndsWith("InternalCommand")))
            {
                CommandModel command = new CommandModel();
                command.Name = typeToProcess.Name;
                command.FullType = typeToProcess.FullName;

                GetCommandParameters(typeToProcess, command);
                GetCommandChecks(typeToProcess, command);

                this.commands.Add(command);

                return true;
            }

            return false;
        }

        private void GetCommandChecks(TypeDefinition typeToProcess, CommandModel command)
        {
            // see if there are any checks on this command
            foreach (var commandInterface in typeToProcess.Interfaces)
            {
                var commandChecks = this.checks.Where(x => x.Name == commandInterface.Name);
                foreach (var commandCheck in commandChecks)
                {
                    command.Checks.Add(commandCheck);
                }
            }
        }

        private void GetCommandParameters(TypeDefinition typeToProcess, CommandModel command)
        {
            foreach (var property in typeToProcess.Properties)
            {
                Property prop = PropertyTypeFiller.FillProperty(property);
                prop.IsSAHLModel = models.Select(x => x.Name).Contains(prop.Type);
                prop.IsCollectionSAHLModel = models.Select(x => x.Name).Contains(prop.CollectionPropertyType);

                var emumerator = enumerations.FirstOrDefault(x => x.Name == prop.Type);
                if (emumerator != null)
                {
                    prop.IsSAHLEnum = true;
                    emumerator.IsUsed = true;
                }
                command.Properties.Add(prop);
            }
        }

    }
}