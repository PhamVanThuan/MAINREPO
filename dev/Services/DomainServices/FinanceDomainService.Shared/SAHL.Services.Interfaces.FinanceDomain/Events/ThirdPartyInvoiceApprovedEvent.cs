using SAHL.Core.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceApprovedEvent : Event
    {
        public ThirdPartyInvoiceModel ApprovedThirdPartyInvoice { get; protected set; }

        public decimal ApprovedInvoiceAmount { get; protected set; }

        public string ApproverUserName { get; protected set; }

        public string ApproverUserCapability { get; protected set; }

        public ThirdPartyInvoiceApprovedEvent
        (
              DateTime date
            , ThirdPartyInvoiceModel approvedThirdPartyInvoice
            , decimal approvedInvoiceAmount
            , string approverUserName
            , string approverUserCapability
        )
            : base(date)
        {
            this.ApprovedThirdPartyInvoice = approvedThirdPartyInvoice;
            this.ApprovedInvoiceAmount = approvedInvoiceAmount;
            this.ApproverUserName = approverUserName;
            this.ApproverUserCapability = approverUserCapability;
        }
    }
}