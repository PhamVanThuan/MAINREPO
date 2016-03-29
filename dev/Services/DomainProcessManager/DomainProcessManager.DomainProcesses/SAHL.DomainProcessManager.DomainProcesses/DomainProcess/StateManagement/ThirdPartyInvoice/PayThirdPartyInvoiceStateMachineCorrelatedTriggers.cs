using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice
{
    public partial class PayThirdPartyInvoiceStateMachine
    {
        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> BatchReferenceSetTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchReadyForProcessingTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> InvoiceAddedToPaymentsTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchAddedToPaymentsTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> InvoiceTransactionPostedTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchTransactionsPostedTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> PaymentBatchProcessedTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchArchivedTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> CompletionConfirmedTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchPaidWithSomeStuckInvoicesEventTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> PartialCompletionConfirmedTrigger { get; protected set; }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> CriticalErrorReportedTrigger { get; protected set; }

        public void FireStateMachineTriggerWithKey(dynamic triggerWithIntParameter, Guid guid, int parameter)
        {
            if (!statesBreadCrumb.ContainsKey(guid))
            {
                Machine.Fire(triggerWithIntParameter, guid, parameter);
            }
        }

        public void FireStateMachineTrigger(dynamic trigger, Guid guid)
        {
            if (!statesBreadCrumb.ContainsKey(guid))
            {
                Machine.Fire<Guid>(trigger, guid);
            }
        }

        private void SetupCommandCorrelatedTriggers(SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger> machine)
        {
            BatchReferenceSetTrigger = machine.SetTriggerParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.BatchReferenceReceived);

            BatchReadyForProcessingTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.BatchReadyForProcessingConfirmation);

            InvoiceAddedToPaymentsTrigger = machine.SetTriggerParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.InvoiceAddedToPaymentEvent);

            BatchAddedToPaymentsTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.BatchAddedToPaymentsConfirmation);

            InvoiceTransactionPostedTrigger = machine.SetTriggerParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent);

            BatchTransactionsPostedTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.PaymentBatchTransactionsPostedConfirmation);

            PaymentBatchProcessedTrigger = machine.SetTriggerParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.PaymentBatchProcessedEvent);

            BatchArchivedTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.PaymentBatchArchivedConfirmation);

            CompletionConfirmedTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.CompletionConfirmed);

            BatchPaidWithSomeStuckInvoicesEventTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.BatchPaidWithSomeStuckInvoicesEvent);

            PartialCompletionConfirmedTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.PartialCompletionConfirmed);

            CriticalErrorReportedTrigger = machine.SetTriggerParameters<Guid>(InvoicePaymentStateTransitionTrigger.CriticalErrorReported);
        }
    }
}