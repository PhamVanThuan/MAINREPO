using SAHL.Core.Data;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Concurrent;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice
{
    public interface IPayThirdPartyInvoiceStateMachine : IDataModel
    {
        int BatchReference { get; }

        ConcurrentQueue<ISystemMessageCollection> SystemMessagesQueue { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger> Machine { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger> InitializeMachine(InvoicePaymentProcessState state);

        void AggregateMessages(ISystemMessageCollection systemMessages);

        ConcurrentQueue<int> StuckInvoiceQueue { get; }

        void CreateStateMachine(PayThirdPartyInvoiceProcessModel thirdPartyInvoices, Guid guid);

        void FireStateMachineTrigger(dynamic trigger, Guid guid);

        void FireStateMachineTriggerWithKey(dynamic triggerWithIntParameter, Guid guid, int parameter);

        bool IsInState(InvoicePaymentProcessState state);

        void RecordRequestSent(Type requestType, Guid requestCorrelationGuid);

        void RecordResponseOrEventReceived(Guid guid);

        void RecordErrorResponseOrCommandFailed(Guid guid);

        int GetStateHistoryCount(InvoicePaymentProcessState state);

        bool AllRequestsBeenServiced();

        void ClearStuckInvoiceQueue();


        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> BatchReferenceSetTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchReadyForProcessingTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> InvoiceAddedToPaymentsTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchAddedToPaymentsTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> InvoiceTransactionPostedTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchTransactionsPostedTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int> PaymentBatchProcessedTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchArchivedTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> CompletionConfirmedTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> BatchPaidWithSomeStuckInvoicesEventTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> PartialCompletionConfirmedTrigger { get; }

        SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid> CriticalErrorReportedTrigger { get; }
    }
}