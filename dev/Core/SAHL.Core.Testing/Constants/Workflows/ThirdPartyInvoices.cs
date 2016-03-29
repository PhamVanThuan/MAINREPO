using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Testing.Constants.Workflows
{
    public class ThirdPartyInvoices
    {
        public class States
        {
            public const string LossControlInvoiceReceived = "Loss Control Invoice Received";
            public const string OtherInvoiceReceived = "Other Invoice Received";
            public const string InvoiceRejected = "Invoice Rejected";
            public const string AcceptForProcessing = "Accepted for Processing";
            public const string InvoiceCaptured = "Invoice Captured";
            public const string AwaitingApprovalForPayment = "Awaiting Approval for Payment";
            public const string AwaitingPaymentAuthorisation = "Awaiting Payment Authorisation";
            public const string ReadyForProcessing = "Ready for Processing";
        }

        public class Activities
        {
            public const string AcceptInvoice = "Accept Invoice";
            public const string CaptureInvoice = "Capture Invoice";
            public const string QueryOnInvoice = "Query on Invoice";
            public const string AmendInvoice = "Amend Invoice";
            public const string EscalateForApproval = "Escalate for Approval";
            public const string ApproveForPayment = "Approve for Payment";
            public const string Pay = "Pay";
            public const string ReassignProcessor = "Reassign Processor";
            public const string RejectInvoice = "Reject Invoice";
        }
    }
}
