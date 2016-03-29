using Mono.Cecil;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class CommandHandlerTypeScanner : ITypeScanner
    {
        private List<CommandModel> commands;
        private List<EventModel> events;
        private List<RuleModel> rules;

        public CommandHandlerTypeScanner(List<CommandModel> commands, List<EventModel> events, List<RuleModel> rules)
        {
            this.commands = commands;
            this.events = events;
            this.rules = rules;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            // find the command that matches the handler
            if (typeToProcess.Name.EndsWith("CommandHandler"))
            {
                var commandHandlerInterfaces = typeToProcess.Interfaces.Where(x => x.FullName.StartsWith("SAHL.Core.Services.IDomainServiceCommandHandler`2<"));
                foreach (var commandHandlerInterface in commandHandlerInterfaces)
                {
                    var commandTypeInterface = commandHandlerInterface as Mono.Cecil.GenericInstanceType;
                    var commandType = commandTypeInterface.GenericArguments[0];

                    var command = this.commands.Where(x => x.Name == commandType.Name).SingleOrDefault();
                    GetCommandHandlerEvents(command, commandTypeInterface);
                    GetCommandHanderRules(typeToProcess, command);
                }
            }

            return false;
        }

        private void GetCommandHandlerEvents(CommandModel command, GenericInstanceType commandTypeInt)
        {
            if (command != null)
            {
                var eventType = commandTypeInt.GenericArguments[1];
                var @event = this.events.Where(x => x.Name == eventType.Name).SingleOrDefault();
                if (@event != null)
                {
                    command.RaisedEvent = @event;
                }
            }
        }

        private void GetCommandHanderRules(TypeDefinition typeToProcess, CommandModel command)
        {
            // look for domain rules
            // first get the constuctor
            var constuctors = typeToProcess.Methods.Where(x => x.IsConstructor == true);
            foreach (var constructor in constuctors)
            {
                if (constructor.HasBody)
                {
                    foreach (var instruction in constructor.Body.Instructions)
                    {
                        if (instruction.OpCode.Code == Mono.Cecil.Cil.Code.Callvirt)
                        {
                            var methodRef = instruction.Operand as Mono.Cecil.MethodReference;
                            string[] ruleMethods = new string[] { "RegisterRule", "RegisterPartialRule", "RegisterRuleForContext" };
                            if (methodRef.FullName.StartsWith("System.Void SAHL.Core.Rules.IDomainRuleManager`1<") &&
                                ruleMethods.Contains(methodRef.Name))
                            {
                                // we have a rule being registered
                                var paramType = methodRef.Parameters[0].ParameterType as GenericInstanceType;
                                var ruleGeneric = paramType.GenericArguments[0].GetElementType();
                                if (ruleGeneric.MetadataType == MetadataType.Var)
                                {
                                    // look to the previous instruction
                                    if (instruction.Previous.OpCode.Code == Mono.Cecil.Cil.Code.Newobj)
                                    {
                                        var newMethodRef = instruction.Previous.Operand as Mono.Cecil.MethodReference;
                                        var ruleDef = newMethodRef.DeclaringType;
                                        var rule = this.rules.Where(x => x.Name == ruleDef.Name).SingleOrDefault();
                                        if (rule != null)
                                        {
                                            command.Rules.Add(rule);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}