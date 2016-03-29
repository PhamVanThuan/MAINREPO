using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class AddNewUserCommandHandler : IServiceCommandHandler<AddNewUserCommand>
    {
        private ISecurityManager securityManager;

        public AddNewUserCommandHandler(ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddNewUserCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            securityManager.AddUser(command.Username, command.EmailAddress, command.FirstName, command.LastName, command.RolesToAssign, command.BranchId);
            return messages;
        }
    }
}