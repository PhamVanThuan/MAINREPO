using System;
using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DocumentManager.Models;

namespace SAHL.Services.Interfaces.DocumentManager.Commands
{
    public class StoreAttorneyInvoiceCommand : ServiceCommand, IDocumentManagerCommand
    {

        private const int storeId = 44;

        public int StoreId
        {
            get { return storeId; }
        }

        public string ThirdPartyInvoiceKey { get; protected set; }

        public ThirdPartyInvoiceDocumentModel AttorneyInvoiceDocument { get; protected set; }

        public StoreAttorneyInvoiceCommand(ThirdPartyInvoiceDocumentModel attorneyInvoiceDocument, string thirdPartyInvoiceKey)
        {
            this.AttorneyInvoiceDocument = attorneyInvoiceDocument;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

    }
}