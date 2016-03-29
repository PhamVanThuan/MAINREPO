using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.Variable;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveAndPublishVariableSetCommandHandler : IServiceCommandHandler<SaveAndPublishVariableSetCommand>
    {
        private IVariableManager variableManager;

        public SaveAndPublishVariableSetCommandHandler(IVariableManager variableManager)
        {
            this.variableManager = variableManager;
        }

        public ISystemMessageCollection HandleCommand(SaveAndPublishVariableSetCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            variableManager.SaveAndPublishVariableSet(command.VariableSetID, command.Version, command.Data, command.Publisher);
            return messages;
        }
    }
}