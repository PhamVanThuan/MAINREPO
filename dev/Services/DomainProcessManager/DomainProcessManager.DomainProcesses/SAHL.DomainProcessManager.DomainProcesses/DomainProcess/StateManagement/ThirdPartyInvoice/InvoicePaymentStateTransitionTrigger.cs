namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice
{
    public enum InvoicePaymentStateTransitionTrigger
    {
        BatchReferenceReceived
      , BatchReadyForProcessingConfirmation
      , InvoiceAddedToPaymentEvent
      , BatchAddedToPaymentsConfirmation
      , InvoiceTransactionPostedEvent
      , PaymentBatchTransactionsPostedConfirmation
      , PaymentBatchArchivedConfirmation
      , BatchPaidWithSomeStuckInvoicesEvent
      , PaymentBatchProcessedEvent
      , PartialCompletionConfirmed
      , CompletionConfirmed
      , CriticalErrorReported
    }
}