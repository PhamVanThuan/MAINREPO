using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveMRUDecisionTreeCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public SaveMRUDecisionTreeCommand(string userName, Guid treeId)
        {
            this.UserName = userName;
            this.TreeId = treeId;
        }

        public string UserName { get; protected set; }

        public Guid TreeId { get; protected set; } 
    }
}