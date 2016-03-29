namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice
{
    public enum InvoicePaymentProcessState
    {
        StartingPaymentProcess
        ,BatchReferenceSet
        ,WorkflowCasesPreparedForPayment
        ,InvoiceAddedToPayments
        ,BatchAddedToPayments
        ,InvoiceTransactionPosted
        ,TransactionsPostedForBatch
        ,WorkflowCasesArchived
        ,BatchProcessed
        ,CompletedPartially
        ,Completed
        ,PaymentBatchFailed
        ,CriticalErrorOccured
    }
}