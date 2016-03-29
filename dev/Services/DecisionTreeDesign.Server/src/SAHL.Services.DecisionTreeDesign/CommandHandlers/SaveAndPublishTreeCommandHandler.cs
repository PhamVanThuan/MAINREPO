using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveAndPublishDecisionTreeCommandHandler : IServiceCommandHandler<SaveAndPublishDecisionTreeCommand>
    {
        private IDecisionTreeManager decisionTreeManager;

        public SaveAndPublishDecisionTreeCommandHandler(IDecisionTreeManager decisionTreeManager)
        {
            this.decisionTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(SaveAndPublishDecisionTreeCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            decisionTreeManager.SaveAndPublishDecisionTree(command.DecisionTreeId, command.Name, command.Description, command.IsActive, command.Thumbnail, command.TreeVersionId, command.Data, command.Publisher, command.SaveFirst);
            return messages;
        }
    }
}