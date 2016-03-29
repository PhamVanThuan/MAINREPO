using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.MRUTree;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class SaveMRUDecisionTreePinStatusCommandHandler : IServiceCommandHandler<SaveMRUDecisionTreePinStatusCommand>
    {
        private IMRUTreeManager MRUTreeManager;

        public SaveMRUDecisionTreePinStatusCommandHandler(IMRUTreeManager decisionTreeManager)
        {
            this.MRUTreeManager = decisionTreeManager;
        }

        public ISystemMessageCollection HandleCommand(SaveMRUDecisionTreePinStatusCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            this.MRUTreeManager.SaveMRUDecisionTreesPinStatus(command.TreeId, command.UserName, command.Pinned);
            return messages;
        }
    }
}