namespace SAHL.Core.BusinessModel.Enums
{
    public enum Capability
    {
        InvoiceProcessor = 1,
        LossControlFeeInvoiceApproverUnderR15000 = 2,
        LossControlFeeInvoiceApproverUnderR30000 = 3,
        LossControlFeeInvoiceApproverUptoR60000 = 4,
        InvoiceApproverOverR60000 = 5,
        InvoicePaymentProcessor = 6,
        InvoiceApprover = 7,
        LossControlFeeConsultant = 8
    }
}