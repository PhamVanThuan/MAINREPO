using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.DocumentManager.Commands
{
    public class RemoveAttorneyInvoiceCommand : ServiceCommand, IDocumentManagerCommand
    {
        public  int AttorneyInvoiceKey {get; protected set;}

        public RemoveAttorneyInvoiceCommand(int attorneyInvoiceKey)
        {
            this.AttorneyInvoiceKey = attorneyInvoiceKey;
        }
    }
}