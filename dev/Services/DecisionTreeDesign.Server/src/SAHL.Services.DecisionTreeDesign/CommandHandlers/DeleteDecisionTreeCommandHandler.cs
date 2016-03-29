using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class DeleteDecisionTreeCommandHandler : IServiceCommandHandler<DeleteDecisionTreeCommand>
    {
        private IDecisionTreeManager decisionTreeManager;

        public DeleteDecisionTreeCommandHandler(IDecisionTreeManager decisionTreeManager)
        {
            this.decisionTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(DeleteDecisionTreeCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            decisionTreeManager.DeleteDecisionTree(command.DecisionTreeId, command.DecisionTreeVersionId, command.Username);
            return messages;
        }
    }
}