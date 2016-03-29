using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveMRUDecisionTreePinStatusCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public SaveMRUDecisionTreePinStatusCommand(string userName, Guid treeId, bool pinned)
        {
            this.UserName = userName;
            this.TreeId = treeId;
            this.Pinned = pinned;
        }

        public string UserName { get; protected set; }

        public Guid TreeId { get; protected set; }

        public bool Pinned { get; protected set; } 
    }
}