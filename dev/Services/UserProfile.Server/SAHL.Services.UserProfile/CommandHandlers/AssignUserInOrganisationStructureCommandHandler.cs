using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.UserProfile.Commands;
using SAHL.Services.UserProfile.Managers;

namespace SAHL.Services.UserProfile.CommandHandlers
{
    public class AssignUserInOrganisationStructureCommandHandler : IServiceCommandHandler<AssignUserInOrganisationStructureCommand>
    {
        private readonly IUserOrganisationStructureDataManager dataManager;

        public AssignUserInOrganisationStructureCommandHandler(IUserOrganisationStructureDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(AssignUserInOrganisationStructureCommand command,
            IServiceRequestMetadata metadata)
        {
            dataManager.AddLinkOfAdUserToOrgStructure(command.AdUserKey, command.OrganisationStructureKey);
            return SystemMessageCollection.Empty();
        }
    }
}