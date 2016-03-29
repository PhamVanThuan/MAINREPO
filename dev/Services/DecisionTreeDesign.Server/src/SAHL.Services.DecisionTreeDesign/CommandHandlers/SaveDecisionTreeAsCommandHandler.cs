using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveDecisionTreeAsCommandHandler : IServiceCommandHandler<SaveDecisionTreeAsCommand>
    {
        private IDecisionTreeManager decisionTreeManager;

        public SaveDecisionTreeAsCommandHandler(IDecisionTreeManager decisionTreeManager)
        {
            this.decisionTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(SaveDecisionTreeAsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            // save an initial version of the tree
            this.decisionTreeManager.SaveDecisionTreeAs(command.DecisionTreeId, command.Name, command.Description, command.IsActive, command.Thumbnail, command.DecisionTreeVersionId, command.Data, command.Username);
            return messages;
        }
    }
}