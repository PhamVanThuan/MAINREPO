using System;

namespace SAHL.Services.Interfaces.Search.Models
{
    public class SearchForThirdPartyInvoicesQueryResult
    {
        public int ThirdPartyInvoiceKey { get; set; }

        public string SahlReference { get; set; }

        public string InvoiceStatusDescription { get; set; }

        public int AccountKey { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string ReceivedFromEmailAddress { get; set; }

        public string AmountExcludingVAT { get; set; }

        public string VATAmount { get; set; }

        public string TotalAmountIncludingVAT { get; set; }

        public string CapitaliseInvoice { get; set; }

        public DateTime ReceivedDate { get; set; }

        public string SpvDescription { get; set; }

        public string[] WorkflowProcess { get; set; }

        public string[] WorkflowStage { get; set; }

        public string AssignedTo { get; set; }

        public string ThirdParty { get; set; }

        public string InstanceID { get; set; }

        public string GenericKey { get; set; }

        public Guid? DocumentGuid { get; set; }

    }
}