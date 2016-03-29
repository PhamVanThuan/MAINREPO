using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.MRUTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveMRUDecisionTreeCommandHandler : IServiceCommandHandler<SaveMRUDecisionTreeCommand>
    {
        private IMRUTreeManager MRUTreeManager;

        public SaveMRUDecisionTreeCommandHandler(IMRUTreeManager decisionTreeManager)
        {
            this.MRUTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(SaveMRUDecisionTreeCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            this.MRUTreeManager.UpdateMRUDecisionTrees(command.TreeId, command.UserName);
            return messages;
        }
    }
}