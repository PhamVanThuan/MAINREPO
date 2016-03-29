using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveThumbnailCommandHandler : IServiceCommandHandler<SaveThumbnailCommand>
    {
        private IDecisionTreeManager decisionTreeManager;

        public SaveThumbnailCommandHandler(IDecisionTreeManager decisionTreeManager)
        {
            this.decisionTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(SaveThumbnailCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            // save an initial version of the tree
            this.decisionTreeManager.SaveDecisionTreeThumbnail(command.DecisionTreeId, command.Thumbnail);
            return messages;
        }
    }
}