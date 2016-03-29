using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.DocumentManager.Commands
{
    public class StoreClientFileDocumentsCommand : ServiceCommand, IDocumentManagerCommand
    {
        public List<ClientFileDocumentModel> ClientFileDocuments { get; protected set; }

        public StoreClientFileDocumentsCommand(List<ClientFileDocumentModel> clientFileDocuments)
        {
            this.ClientFileDocuments = clientFileDocuments;
        }
    }
}