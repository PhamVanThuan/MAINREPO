using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.CommandHandlers
{
    public class CloseLockForMessagesCommandHandler : IServiceCommandHandler<CloseLockForMessagesCommand>
    {
        public ICurrentlyOpenDocumentManager currentlyOpenDocumentManager;

        public CloseLockForMessagesCommandHandler(ICurrentlyOpenDocumentManager currentlyOpenDocumentManager)
        {
            this.currentlyOpenDocumentManager = currentlyOpenDocumentManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(CloseLockForMessagesCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            currentlyOpenDocumentManager.CloseDocument(command.DocumentVersionId, command.Username);
            return messages;
        }
    }
}
