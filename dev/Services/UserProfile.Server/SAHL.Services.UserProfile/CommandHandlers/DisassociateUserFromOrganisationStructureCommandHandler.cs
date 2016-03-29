using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.UserProfile.Commands;
using SAHL.Services.UserProfile.Managers;

namespace SAHL.Services.UserProfile.CommandHandlers
{
    public class DisassociateUserFromOrganisationStructureCommandHandler : IServiceCommandHandler<DissociateUserFromOrganisationStructureCommand>
    {
        private readonly IUserOrganisationStructureDataManager dataManager;

        public DisassociateUserFromOrganisationStructureCommandHandler(IUserOrganisationStructureDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(DissociateUserFromOrganisationStructureCommand command,IServiceRequestMetadata metadata)
        {
            dataManager.RemoveLinkOfAdUserToOrgStructure(command.AdUserKey, command.OrganisationStructureKey);
            return SystemMessageCollection.Empty();
        }
    }
}