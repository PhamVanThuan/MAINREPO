using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.MessageSet;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveMessageSetCommandHandler : IServiceCommandHandler<SaveMessageSetCommand>
    {
        private IMessageSetManager messageSetDataManager;

        public SaveMessageSetCommandHandler(IMessageSetManager messageSetDataManager)
        {
            this.messageSetDataManager = messageSetDataManager;
        }

        public ISystemMessageCollection HandleCommand(SaveMessageSetCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            messageSetDataManager.SaveMessageSet(command.MessageSetId, command.Version, command.Data);
            return messages;
        }
    }
}