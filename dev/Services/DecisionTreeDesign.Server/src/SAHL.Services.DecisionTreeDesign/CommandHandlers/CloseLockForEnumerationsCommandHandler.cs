using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class CloseLockForEnumerationsCommandHandler : IServiceCommandHandler<CloseLockForEnumerationsCommand>
    {
        public ICurrentlyOpenDocumentManager currentlyOpenDocumentManager;

        public CloseLockForEnumerationsCommandHandler(ICurrentlyOpenDocumentManager currentlyOpenDocumentManager)
        {
            this.currentlyOpenDocumentManager = currentlyOpenDocumentManager;
        }

        public ISystemMessageCollection HandleCommand(CloseLockForEnumerationsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            currentlyOpenDocumentManager.CloseDocument(command.DocumentVersionId, command.Username);
            return messages;
        }
    }
}
