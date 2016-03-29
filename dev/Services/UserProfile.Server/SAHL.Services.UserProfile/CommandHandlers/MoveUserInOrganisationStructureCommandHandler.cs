using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.UserProfile.Commands;
using SAHL.Services.UserProfile.Managers;

namespace SAHL.Services.UserProfile.CommandHandlers
{
    public class MoveUserInOrganisationStructureCommandHandler : IServiceCommandHandler<MoveUserInOrganisationStructureCommand>
    {
        private readonly IUserOrganisationStructureDataManager manager;

        public MoveUserInOrganisationStructureCommandHandler(IUserOrganisationStructureDataManager manager)
        {
            this.manager = manager;
        }

        public ISystemMessageCollection HandleCommand(MoveUserInOrganisationStructureCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessageCollection = SystemMessageCollection.Empty();

            manager.MoveUserWithinOrganisationStructure(command.AdUserKey, command.FromOrganisationStructureKey,
                command.ToOrganisationStructureKey);

            return systemMessageCollection;
        }
    }
}