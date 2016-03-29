using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.MessageSet;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveAndPublishMessageSetCommandHandler : IServiceCommandHandler<SaveAndPublishMessageSetCommand>
    {
        private IMessageSetManager messageSetDataManager;

        public SaveAndPublishMessageSetCommandHandler(IMessageSetManager messageSetDataManager)
        {
            this.messageSetDataManager = messageSetDataManager;
        }

        public ISystemMessageCollection HandleCommand(SaveAndPublishMessageSetCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            messageSetDataManager.SaveAndPublishMessageSet(command.MessageSetId, command.Version, command.Data, command.Publisher);
            return messages;
        }
    }
}