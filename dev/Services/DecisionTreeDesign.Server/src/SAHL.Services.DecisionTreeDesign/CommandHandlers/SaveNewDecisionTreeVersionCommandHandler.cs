using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveNewDecisionTreeVersionCommandHandler : IServiceCommandHandler<SaveNewDecisionTreeVersionCommand>
    {
        private IDecisionTreeManager decisionTreeManager;

        public SaveNewDecisionTreeVersionCommandHandler(IDecisionTreeManager decisionTreeManager)
        {
            this.decisionTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(SaveNewDecisionTreeVersionCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            //Guid decisionTreeVersionId, Guid decisionTreeId, int version, string data, string username
            decisionTreeManager.SaveNewDecisionTreeVersion(command.DecisionTreeVersionId, command.DecisionTreeId, command.Version, command.Data, command.Username);
            return messages;
        }
    }
}