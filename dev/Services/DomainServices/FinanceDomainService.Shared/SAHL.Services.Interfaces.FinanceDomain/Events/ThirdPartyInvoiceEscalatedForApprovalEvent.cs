using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceEscalatedForApprovalEvent : Event
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public int EscalatedBy { get; protected set; }

        public int EscalatedTo { get; set; }

        public ThirdPartyInvoiceEscalatedForApprovalEvent(DateTime date, int thirdPartyInvoiceKey, int escalatedBy, int escalatedTo) : base(date)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.EscalatedBy = escalatedBy;
            this.EscalatedTo = escalatedTo;
        }
    }
}