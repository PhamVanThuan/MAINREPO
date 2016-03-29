using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class OpenLockForTreeCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public Guid DocumentVersionId { get; protected set; }
        public string Username { get; protected set; }

        public OpenLockForTreeCommand(Guid documentVersionId, string username)
        {
            this.DocumentVersionId = documentVersionId;
            this.Username = username;
        }
    }
}
