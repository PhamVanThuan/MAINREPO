using SAHL.Core.Data.Models.DecisionTree;
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
    public class OpenLockForVariablesCommandHandler : IServiceCommandHandler<OpenLockForVariablesCommand>
    {
        public ICurrentlyOpenDocumentManager currentlyOpenDocumentManager;

        public OpenLockForVariablesCommandHandler(ICurrentlyOpenDocumentManager currentlyOpenDocumentManager)
        {
            this.currentlyOpenDocumentManager = currentlyOpenDocumentManager;
        }

        public ISystemMessageCollection HandleCommand(OpenLockForVariablesCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            currentlyOpenDocumentManager.OpenDocument(command.DocumentVersionId, Guid.Parse(DocumentTypeEnumDataModel.VARIABLE_SET), command.Username);
            return messages;
        }
    }
}
