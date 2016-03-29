using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.Variable;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveVariableSetCommandHandler : IServiceCommandHandler<SaveVariableSetCommand>
    {
        private IVariableManager variableManager;

        public SaveVariableSetCommandHandler(IVariableManager variableManager)
        {
            this.variableManager = variableManager;
        }

        public ISystemMessageCollection HandleCommand(SaveVariableSetCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            variableManager.SaveVariableSet(command.Id, command.Version, command.Data);
            return messages;
        }
    }
}