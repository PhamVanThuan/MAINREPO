using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveEnumerationSetCommandHandler : IServiceCommandHandler<SaveEnumerationSetCommand>
    {
        private IEnumerationSetManager enumerationSetManager;

        public SaveEnumerationSetCommandHandler(IEnumerationSetManager enumerationSetManager)
        {
            this.enumerationSetManager = enumerationSetManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(SaveEnumerationSetCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            enumerationSetManager.SaveEnumerationSet(command.Id, command.Version, command.Data);
            return messages;
        }
    }
}