using SAHL.Core.Data;
using SAHL.Core.DomainProcess;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public abstract partial class PayThirdPartyInvoiceDomainProcess<T> : DomainProcessBase<T>
        where T : PayThirdPartyInvoiceProcessModel
    {
        protected IPayThirdPartyInvoiceStateMachine payThirdPartyInvoiceStateMachine;
        protected readonly IFinanceDomainServiceClient financeDomainService;
        protected readonly ICombGuid combGuidGenerator;
        protected readonly ICATSServiceClient catsService;
        protected readonly IX2WorkflowManager x2WorkflowManager;
        protected readonly ICommunicationManager communicationManager;

        public PayThirdPartyInvoiceDomainProcess(
              IPayThirdPartyInvoiceStateMachine payThirdPartyInvoiceStateMachine
            , IFinanceDomainServiceClient financeDomainService
            , ICombGuid combGuidGenerator
            , ICATSServiceClient catsService
            , IX2WorkflowManager x2WorkflowManager
            , ICommunicationManager communicationManager
            , IRawLogger rawLogger
            , ILoggerSource loggerSource
            , ILoggerAppSource loggerAppSource
            )
            : base(rawLogger, loggerSource, loggerAppSource)
        {
            if (payThirdPartyInvoiceStateMachine == null) { throw new ArgumentNullException("payThirdPartyInvoiceStateMachine"); }
            if (financeDomainService == null) { throw new ArgumentNullException("financeDomainService"); }
            if (catsService == null) { throw new ArgumentNullException("catsService"); }
            if (x2WorkflowManager == null) { throw new ArgumentNullException("x2WorkflowManager"); }
            if (communicationManager == null) { throw new ArgumentNullException("communicationManager"); }
            if (combGuidGenerator == null) { throw new ArgumentNullException("combGuidGenerator"); }

            this.catsService = catsService;
            this.combGuidGenerator = combGuidGenerator;
            this.financeDomainService = financeDomainService;
            this.x2WorkflowManager = x2WorkflowManager;
            this.communicationManager = communicationManager;
            this.payThirdPartyInvoiceStateMachine = payThirdPartyInvoiceStateMachine;
        }

        public override void Initialise(IDataModel dataModel)
        {
            if (dataModel == null) { throw new ArgumentNullException("dataModel"); }
            if (!(dataModel is PayThirdPartyInvoiceProcessModel))
            {
                throw new Exception(string.Format("Invalid Data Model. Expected {0} but received {1}", typeof(PayThirdPartyInvoiceProcessModel).Name, dataModel.GetType().Name));
            }

            payThirdPartyInvoiceStateMachine.CreateStateMachine((PayThirdPartyInvoiceProcessModel)dataModel, this.DomainProcessId);
            base.ProcessState = payThirdPartyInvoiceStateMachine;
        }

        public override void RestoreState(IDataModel stateMachineMissingTriggers)
        {
            if (stateMachineMissingTriggers == null) { throw new ArgumentNullException("dataModel"); }
            if (!(stateMachineMissingTriggers is PayThirdPartyInvoiceStateMachine))
            {
                throw new Exception(string.Format("Invalid Data Model. Expected {0} but received {1}", typeof(PayThirdPartyInvoiceStateMachine).Name, stateMachineMissingTriggers.GetType().Name));
            }
            PayThirdPartyInvoiceStateMachine stateMachine = stateMachineMissingTriggers as PayThirdPartyInvoiceStateMachine;
            stateMachine.InitializeMachine(stateMachine.State);
            this.payThirdPartyInvoiceStateMachine = stateMachine;
            this.ProcessState = stateMachine;
        }

        public override void HandledEvent(IServiceRequestMetadata metadata)
        {
            if (this.ProcessState != null)
            {
                var payThirdPartyInvoiceStateMachine = this.ProcessState as IPayThirdPartyInvoiceStateMachine;
                payThirdPartyInvoiceStateMachine.RecordResponseOrEventReceived(Guid.Parse(metadata[DomainProcessManagerGlobals.CommandCorrelationKey]));

                if (payThirdPartyInvoiceStateMachine.AllRequestsBeenServiced())
                {
                    CompleteDomainProcess();
                }
            }
        }

        protected void CompleteDomainProcess()
        {
            var stuckMessages = payThirdPartyInvoiceStateMachine.SystemMessagesQueue.SelectMany(x => x.AllMessages).Select(x => x as ISystemMessage).ToList();
            var msgs = new SystemMessageCollection(stuckMessages);

            if (payThirdPartyInvoiceStateMachine.IsInState(InvoicePaymentProcessState.Completed))
            {
                OnCompleted(this.DomainProcessId);
            }
            else if (payThirdPartyInvoiceStateMachine.IsInState(InvoicePaymentProcessState.CompletedPartially))
            {
                OnPartialComplete(msgs);
            }
            else
            {
                CriticalErrorOccured(msgs);
            }
            CompensateStuckInvoices();
        }

        public void CompensateStuckInvoices()
        {
            var manualArchiveQueue = new ConcurrentQueue<string>();
            Parallel.ForEach(payThirdPartyInvoiceStateMachine.StuckInvoiceQueue, (invoiceKey) =>
            {
                var invoice = this.DataModel.InvoiceCollection.First(y => y.ThirdPartyInvoiceKey == invoiceKey);
                switch (invoice.StepInProcess)
                {
                    case PaymentProcessStep.ArchivingFailed:
                        manualArchiveQueue.Enqueue(invoice.SAHLReference);
                        break;

                    case PaymentProcessStep.BatchingFailed:
                        ReturnWorkflowCaseToPaymentApproved(invoiceKey);
                        break;

                    case PaymentProcessStep.PostingTransactionFailed:
                        RemoveInvoiceFromPaymentBatch(invoiceKey);
                        ReturnWorkflowCaseToPaymentApproved(invoiceKey);
                        break;

                    case PaymentProcessStep.PaymentBatchFailed:
                        ReverseTransaction(invoiceKey);
                        RemoveInvoiceFromPaymentBatch(invoiceKey);
                        MarkCATSPaymentBatchAsFailed();
                        ReturnWorkflowCaseToPaymentApproved(invoiceKey);
                        break;

                    case PaymentProcessStep.PreparingWorkflowCaseFailed:
                    default:
                        break;
                }
            });

            Parallel.ForEach(payThirdPartyInvoiceStateMachine.SystemMessagesQueue, (systemMessages) =>
            {
                if (systemMessages.ErrorMessages().Count() > 1)
                {
                    this.LogErrorMessage(systemMessages.ErrorMessages().Select(h => h.Message).Aggregate((i, j) => i + j));
                }
                else if (systemMessages.ErrorMessages().Count() == 1)
                {
                    this.LogErrorMessage(systemMessages.ErrorMessages().First().Message);
                }
            });

            SendOperatorManualArchiveRequest(manualArchiveQueue.ToList());
            payThirdPartyInvoiceStateMachine.ClearStuckInvoiceQueue();
        }

        private void MarkCATSPaymentBatchAsFailed()
        {
            var messages = SystemMessageCollection.Empty();
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var markCATSPaymentBatchAsFailedCommand = new MarkCATSPaymentBatchAsFailedCommand(payThirdPartyInvoiceStateMachine.BatchReference);
            messages.Aggregate(catsService.PerformCommand(markCATSPaymentBatchAsFailedCommand, metadata));
        }

        private void RemoveInvoiceFromPaymentBatch(int invoiceKey)
        {
            var messages = SystemMessageCollection.Empty();
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var removeThirdPartyInvoiceFromPaymentBatchCommand = new RemoveThirdPartyInvoiceFromPaymentBatchCommand(payThirdPartyInvoiceStateMachine.BatchReference, invoiceKey);
            messages.Aggregate(financeDomainService.PerformCommand(removeThirdPartyInvoiceFromPaymentBatchCommand, metadata));
            if (messages.HasErrors)
            {
                // Crisis - retrying might help
            }
        }

        private void ReverseTransaction(int invoiceKey)
        {
            var messages = SystemMessageCollection.Empty();
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var postTransactionReversalCommand = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(invoiceKey);
            messages.Aggregate(financeDomainService.PerformCommand(postTransactionReversalCommand, metadata));
            if (messages.HasErrors)
            {
                // Crisis - retrying might help
            }
        }

        private void ReturnWorkflowCaseToPaymentApproved(int invoiceKey)
        {
            var messages = SystemMessageCollection.Empty();
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var invoice = this.DataModel.InvoiceCollection.First(y => y.ThirdPartyInvoiceKey == invoiceKey);
            messages.Aggregate(x2WorkflowManager.ReversePayment(invoice.InstanceId, invoice.AccountNumber, invoice.ThirdPartyInvoiceKey, metadata));
            if (messages.HasErrors)
            {
                // Crisis - retrying might help
            }
        }

        private void SendOperatorManualArchiveRequest(List<string> manualArchiveInvoiceReferences)
        {
            if (manualArchiveInvoiceReferences.Count > 0)
            {
                communicationManager.SendOperatorRequestForManualArchive(manualArchiveInvoiceReferences);
            }
        }

        public void HandleException(IEvent @event, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            payThirdPartyInvoiceStateMachine.RecordResponseOrEventReceived(Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
            if (payThirdPartyInvoiceStateMachine.AllRequestsBeenServiced())
            {
                var stuckMessages = payThirdPartyInvoiceStateMachine.SystemMessagesQueue.SelectMany(x => x.AllMessages).Select(x => x as ISystemMessage).ToList();
                var msgs = new SystemMessageCollection(stuckMessages);
                CriticalErrorOccured(msgs);
                CompensateStuckInvoices();
            }
        }

        protected void CriticalErrorOccured(ISystemMessageCollection systemMessages)
        {
            payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(
                        payThirdPartyInvoiceStateMachine.CriticalErrorReportedTrigger
                        , this.DomainProcessId);
            var errorMessages = new StringBuilder();
            foreach (var message in systemMessages.AllMessages)
            {
                errorMessages.AppendLine(message.Message);
            }
            OnErrorOccurred(this.DomainProcessId, errorMessages.ToString());
            MarkCATSPaymentBatchAsFailed();
        }

        private void OnPartialComplete(ISystemMessageCollection systemMessages)
        {
            var errorMessages = new StringBuilder();
            foreach (var message in systemMessages.AllMessages)
            {
                errorMessages.AppendLine(message.Message);
            }
            OnErrorOccurred(this.DomainProcessId, errorMessages.ToString());
            OnCompleted(this.DomainProcessId);
        }
    }
}