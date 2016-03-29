using System;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.DataStore;
using SAHL.Services.Interfaces.DocumentManager.Commands;

namespace SAHL.Services.DocumentManager.CommandHandlers
{

    public class RemoveAttorneyInvoiceCommandHandler : IServiceCommandHandler<RemoveAttorneyInvoiceCommand>
    {
        private IDocumentDataManager dataManager;

        public RemoveAttorneyInvoiceCommandHandler(IDocumentDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveAttorneyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            dataManager.RemoveAttorneyInvoice(command.AttorneyInvoiceKey);

            return messages;
        }

    }
}