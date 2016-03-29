using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class AddNewBranchCommandHandler : IServiceCommandHandler<AddNewBranchCommand>
    {
        private ISecurityManager securityManager;

        public AddNewBranchCommandHandler(ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
        }

        public ISystemMessageCollection HandleCommand(AddNewBranchCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            securityManager.AddBranch(command.BranchName, command.IsActive, command.SuburbId, command.BranchCode);
            return messages; ;
        }
    }
}