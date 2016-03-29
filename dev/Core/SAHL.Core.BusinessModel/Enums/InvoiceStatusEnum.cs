namespace SAHL.Core.BusinessModel.Enums
{
    public enum InvoiceStatus
    {
        Received = 1,
        AwaitingApproval = 2,
        Approved = 3,
        ProcessingPayment = 4,
        Rejected = 5,
        Paid = 6,
        Captured = 7
    }
}