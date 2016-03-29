using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ChangeBranchDetailsCommandHandler : IServiceCommandHandler<ChangeBranchDetailsCommand>
    {
        private ISecurityManager securityManager;
        static ServiceRequestMetadata metadata;

        public ChangeBranchDetailsCommandHandler(ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ChangeBranchDetailsCommand command, IServiceRequestMetadata metadatad)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            securityManager.ChangeBrancheDetails(command.Id, command.BranchName, command.IsActive, command.SuburbId);
            return messages;
        }
    }
}