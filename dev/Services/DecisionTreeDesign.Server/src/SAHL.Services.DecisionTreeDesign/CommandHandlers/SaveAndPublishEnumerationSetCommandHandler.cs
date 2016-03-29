using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveAndPublishEnumerationSetCommandHandler : IServiceCommandHandler<SaveAndPublishEnumerationSetCommand>
    {
        private IEnumerationSetManager enumerationSetManager;

        public SaveAndPublishEnumerationSetCommandHandler(IEnumerationSetManager enumerationSetManager)
        {
            this.enumerationSetManager = enumerationSetManager;
        }

        public ISystemMessageCollection HandleCommand(SaveAndPublishEnumerationSetCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            enumerationSetManager.SaveAndPublishEnumerationSet(command.Id, command.Version, command.Data, command.Publisher);
            return messages;
        }
    }
}