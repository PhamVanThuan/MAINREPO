using System.Collections.Generic;
using System.Linq;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.AssociationScanners
{
    public class RuleAssociationScanner : IAssociationScanner
    {
        private List<EventModel> events;
        private List<RuleModel> rules;
        private List<CommandModel> commands;

        public RuleAssociationScanner(List<RuleModel> rules, List<CommandModel> commands)
        {
            this.rules = rules;
            this.commands = commands;
        }

        public void ProcessAssociations()
        {
            foreach (var command in commands)
            {
                foreach (var rule in command.Rules)
                {

                    var currentRule = rules.FirstOrDefault(x => x.Name.Equals(rule.Name));
                    if (currentRule != null)
                    {
                        if (currentRule.ParentCommands.FirstOrDefault(x => x.Name.Equals(command.Name)) == null)
                        {
                            currentRule.ParentCommands.Add(new AssociateItemModel() { Name = command.Name });
                        }
                    }

                }

            }
        }


    }

}