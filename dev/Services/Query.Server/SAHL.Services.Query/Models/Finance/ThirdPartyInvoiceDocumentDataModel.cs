using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Models.Finance
{
    public class ThirdPartyInvoiceDocumentDataModel : IQueryDataModel
    {

        public List<IRelationshipDefinition> Relationships { get; set; }

        public Guid Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? AccountKey { get; set; }
        public int? StorKey { get; set; }
        public int? ThirdPartyInvoiceKey { get; set; }
        public string EmailSubject { get; set; }
        public string FromEmailAddress { get; set; }
        public string InvoiceFileName { get; set; }
        public string Category { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateProcessed { get; set; }
        public int? InvoiceStatusKey { get; set; }
        public string InvoiceStatusDescription { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public int? ThirdPartyKey { get; set; }

        public ThirdPartyInvoiceDocumentDataModel()
        {
            SetupRelationships();
        }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }
    }
}