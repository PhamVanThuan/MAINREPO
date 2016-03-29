using Newtonsoft.Json;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice
{
    public partial class PayThirdPartyInvoiceStateMachine : IPayThirdPartyInvoiceStateMachine
    {
        private InvoicePaymentProcessState defaultInitialState = InvoicePaymentProcessState.StartingPaymentProcess;

        [JsonProperty]
        private Guid domainProcessId;

        [JsonProperty]
        private int batchReferenceNumber;

        [JsonProperty]
        private ConcurrentQueue<int> stuckInvoiceQueue;

        [JsonProperty]
        private SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger> machine;

        [JsonProperty]
        private InvoicePaymentProcessState serializationStateSnapshot;

        [JsonProperty]
        private Dictionary<Guid, InvoicePaymentProcessState> statesBreadCrumb;

        [JsonProperty]
        private ConcurrentDictionary<Guid, Tuple<Type, bool>> sentRequests;

        public ConcurrentQueue<ISystemMessageCollection> SystemMessagesQueue { get; protected set; }

        public int BatchReference
        {
            get { return batchReferenceNumber; }
            protected set { batchReferenceNumber = value; }
        }

        public ConcurrentQueue<int> StuckInvoiceQueue
        {
            get { return stuckInvoiceQueue; }
            protected set { stuckInvoiceQueue = value; }
        }

        private void CreateStateMachine(PayThirdPartyInvoiceProcessModel thirdPartyInvoices, InvoicePaymentProcessState state)
        {
            SystemMessagesQueue = new ConcurrentQueue<ISystemMessageCollection>();
            stuckInvoiceQueue = new ConcurrentQueue<int>();

            sentRequests = new ConcurrentDictionary<Guid, Tuple<Type, bool>>();
            statesBreadCrumb = new Dictionary<Guid, InvoicePaymentProcessState>();

            RecordStateHistory(Guid.NewGuid(), state);
            machine = InitializeMachine(state);
        }

        public void AggregateMessages(ISystemMessageCollection systemMessages)
        {
            throw new System.NotImplementedException();
        }

        public void CreateStateMachine(PayThirdPartyInvoiceProcessModel thirdPartyInvoices, Guid domainProcessId)
        {
            this.domainProcessId = domainProcessId;
            CreateStateMachine(thirdPartyInvoices, defaultInitialState);
        }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger> InitializeMachine(InvoicePaymentProcessState state)
        {
            machine = new SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>(state);
            stuckInvoiceQueue = new ConcurrentQueue<int>();
            SystemMessagesQueue = new ConcurrentQueue<ISystemMessageCollection>();
            SetupCommandCorrelatedTriggers(machine);

            InitializePermittedTransitions(machine);

            return machine;
        }

        private void InitializePermittedTransitions(SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger> machine)
        {
            machine.Configure(InvoicePaymentProcessState.StartingPaymentProcess)
                .Permit(InvoicePaymentStateTransitionTrigger.BatchReferenceReceived, InvoicePaymentProcessState.BatchReferenceSet)
                .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.BatchReferenceSet)
                 .OnEntryFrom(BatchReferenceSetTrigger, (correlationId, referenceNumber) =>
                 {
                     RecordStateHistory(correlationId, InvoicePaymentProcessState.BatchReferenceSet);
                     BatchReference = referenceNumber;
                 })
                .Permit(InvoicePaymentStateTransitionTrigger.BatchReadyForProcessingConfirmation, InvoicePaymentProcessState.WorkflowCasesPreparedForPayment)
                .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.WorkflowCasesPreparedForPayment)
                .OnEntryFrom(BatchReadyForProcessingTrigger, (correlationId) =>
                {
                    RecordStateHistory(correlationId, InvoicePaymentProcessState.WorkflowCasesPreparedForPayment);
                })
                .Permit(InvoicePaymentStateTransitionTrigger.InvoiceAddedToPaymentEvent, InvoicePaymentProcessState.InvoiceAddedToPayments)
                .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.InvoiceAddedToPayments)
                .PermitReentry(InvoicePaymentStateTransitionTrigger.InvoiceAddedToPaymentEvent)
                .OnEntryFrom(InvoiceAddedToPaymentsTrigger, (correlationId, thirdPartyInvoiceKey) =>
                {
                    RecordStateHistory(correlationId, InvoicePaymentProcessState.InvoiceAddedToPayments);
                })
                .Permit(InvoicePaymentStateTransitionTrigger.BatchAddedToPaymentsConfirmation, InvoicePaymentProcessState.BatchAddedToPayments)
                .Permit(InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent, InvoicePaymentProcessState.InvoiceTransactionPosted)
                .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.BatchAddedToPayments)
                .OnEntryFrom(InvoiceAddedToPaymentsTrigger, (correlationId, thirdPartyInvoiceKey) =>
                {
                    RecordStateHistory(correlationId, InvoicePaymentProcessState.BatchAddedToPayments);
                    //TODO: Compensate
                    //TODO: CREATE CATS FILE
                })
                .Permit(InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent, InvoicePaymentProcessState.InvoiceTransactionPosted)
                .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.InvoiceTransactionPosted)
                .PermitReentry(InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent)
                .OnEntryFrom(InvoiceTransactionPostedTrigger, (correlationId, thirdPartyInvoiceKey) =>
                    {
                        RecordStateHistory(correlationId, InvoicePaymentProcessState.InvoiceTransactionPosted);
                    })
                .Permit(InvoicePaymentStateTransitionTrigger.PaymentBatchTransactionsPostedConfirmation, InvoicePaymentProcessState.TransactionsPostedForBatch)
                .Permit(InvoicePaymentStateTransitionTrigger.BatchAddedToPaymentsConfirmation, InvoicePaymentProcessState.BatchAddedToPayments)
                .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.TransactionsPostedForBatch)
                .OnEntryFrom(BatchTransactionsPostedTrigger, (correlationId, thirdPartyInvoiceKey) =>
                {
                    RecordStateHistory(correlationId, InvoicePaymentProcessState.TransactionsPostedForBatch);
                })
                 .Permit(InvoicePaymentStateTransitionTrigger.PaymentBatchProcessedEvent, InvoicePaymentProcessState.BatchProcessed)
                 .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.BatchProcessed)
                .OnEntryFrom(BatchArchivedTrigger, (correlationId) =>
                {
                    RecordStateHistory(correlationId, InvoicePaymentProcessState.BatchProcessed);
                })
                .Permit(InvoicePaymentStateTransitionTrigger.PaymentBatchArchivedConfirmation, InvoicePaymentProcessState.WorkflowCasesArchived)
                .Permit(InvoicePaymentStateTransitionTrigger.PartialCompletionConfirmed, InvoicePaymentProcessState.CompletedPartially)
                .Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.WorkflowCasesArchived)
                .OnEntry(transition =>
                {
                    RecordStateHistory(CombGuid.Instance.Generate(), InvoicePaymentProcessState.WorkflowCasesArchived);
                })
                .PermitIf(InvoicePaymentStateTransitionTrigger.CompletionConfirmed, InvoicePaymentProcessState.Completed, () =>
                {
                    return stuckInvoiceQueue.Count == 0;
                })
                .PermitIf(InvoicePaymentStateTransitionTrigger.PartialCompletionConfirmed, InvoicePaymentProcessState.CompletedPartially, () =>
                {
                    return stuckInvoiceQueue.Count > 0;
                }).Permit(InvoicePaymentStateTransitionTrigger.CriticalErrorReported, InvoicePaymentProcessState.CriticalErrorOccured);

            machine.Configure(InvoicePaymentProcessState.CriticalErrorOccured)
                .OnEntryFrom(CriticalErrorReportedTrigger, (correlationId) =>
                {
                    RecordStateHistory(correlationId, InvoicePaymentProcessState.CriticalErrorOccured);
                });
        }

        public InvoicePaymentProcessState State
        {
            get
            {
                InvoicePaymentProcessState actualState = machine.State;
                if (statesBreadCrumb.Count > 1 && IsInState(InvoicePaymentProcessState.StartingPaymentProcess))
                {
                    actualState = serializationStateSnapshot;
                }
                return actualState;
            }
        }

        public SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger> Machine
        {
            get { return machine; }
        }

        public bool IsInState(InvoicePaymentProcessState state)
        {
            return Machine.State == state;
        }

        public void RecordRequestSent(Type requestType, Guid requestCorrelationGuid)
        {
            sentRequests.TryAdd(requestCorrelationGuid, new Tuple<Type, bool>(requestType, false));
        }

        public void RecordErrorResponseOrCommandFailed(Guid commandCorrelationGuid)
        {
            RecordResponseReceived(commandCorrelationGuid);
        }

        public void RecordResponseOrEventReceived(Guid commandCorrelationGuid)
        {
            RecordResponseReceived(commandCorrelationGuid);
        }

        public bool AllRequestsBeenServiced()
        {
            return sentRequests.All(x => x.Value.Item2);
        }

        private void RecordResponseReceived(Guid commandCorrelationGuid)
        {
            Tuple<Type, bool> sentCommand;
            if (sentRequests.TryGetValue(commandCorrelationGuid, out sentCommand))
            {
                sentRequests[commandCorrelationGuid] = new Tuple<Type, bool>(sentCommand.Item1, true);
            }
        }

        private void RecordStateHistory(Guid guid, InvoicePaymentProcessState state)
        {
            if (!statesBreadCrumb.ContainsKey(guid))
            {
                statesBreadCrumb.Add(guid, state);
                serializationStateSnapshot = state;
            }
        }

        public int GetStateHistoryCount(InvoicePaymentProcessState state)
        {
            return statesBreadCrumb.Where(x => x.Value == state).Count();

        }

        public void ClearStuckInvoiceQueue()
        {
            if (sentRequests.All(r => r.Value.Item2))
            {
                int i;
                while (stuckInvoiceQueue.Count > 0)
                {
                    stuckInvoiceQueue.TryDequeue(out i);
                }
            }
        }

    }
}