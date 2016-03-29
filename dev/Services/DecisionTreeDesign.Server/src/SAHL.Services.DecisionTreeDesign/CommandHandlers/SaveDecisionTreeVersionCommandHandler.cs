using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveDecisionTreeVersionCommandHandler : IServiceCommandHandler<SaveDecisionTreeVersionCommand>
    {
        private IDecisionTreeManager decisionTreeManager;

        public SaveDecisionTreeVersionCommandHandler(IDecisionTreeManager decisionTreeManager)
        {
            this.decisionTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(SaveDecisionTreeVersionCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            decisionTreeManager.SaveDecisionTreeVersion(command.DecisionTreeId, command.Name, command.Description, command.IsActive,command.Thumbnail,command.DecisionTreeVersionId, command.Data, command.Username);
            return messages;
        }
    }
}