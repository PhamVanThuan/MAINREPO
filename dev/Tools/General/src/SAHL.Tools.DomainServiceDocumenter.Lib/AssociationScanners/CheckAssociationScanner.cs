using System.Collections.Generic;
using System.Linq;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.AssociationScanners
{
    public class CheckAssociationScanner : IAssociationScanner
    {

        private List<DomainCheckModel> domainCheckModels;
        private List<CommandModel> commands;

        public CheckAssociationScanner(List<DomainCheckModel> domainCheckModels, List<CommandModel> commands)
        {
            this.domainCheckModels = domainCheckModels;
            this.commands = commands;
        }

        public void ProcessAssociations()
        {
            foreach (var command in commands)
            {
                foreach (var chk in command.Checks)
                {
                    var currentCheck = domainCheckModels.FirstOrDefault(x => x.Name.Equals(chk.Name));
                    if (currentCheck != null)
                    {
                        if (currentCheck.ParentCommands.FirstOrDefault(x => x.Name.Equals(command.Name)) == null)
                        {
                            currentCheck.ParentCommands.Add(new AssociateItemModel() { Name = command.Name });
                        }
                    }

                }

            }
        }
    }
}