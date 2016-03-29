using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ChangeUserDetailsCommandHandler : IServiceCommandHandler<ChangeUserDetailsCommand>
    {
        private ISecurityManager securityManager;

        public ChangeUserDetailsCommandHandler(ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ChangeUserDetailsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            securityManager.ChangeUserDetails(command.Id, command.EmailAddress, command.FirstName, command.LastName, command.Status, command.RolesToAssign, command.RolesToRemove, command.BranchId);
            return messages;
        }
    }
}
