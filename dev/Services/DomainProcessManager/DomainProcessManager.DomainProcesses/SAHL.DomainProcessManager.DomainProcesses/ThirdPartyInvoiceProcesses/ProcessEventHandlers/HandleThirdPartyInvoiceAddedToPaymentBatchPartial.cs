using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public abstract partial class PayThirdPartyInvoiceDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<ThirdPartyInvoiceAddedToBatchEvent>
        where T : PayThirdPartyInvoiceProcessModel
    {
        public void Handle(ThirdPartyInvoiceAddedToBatchEvent invoiceAddedToPaymentBatchEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            payThirdPartyInvoiceStateMachine.FireStateMachineTriggerWithKey(payThirdPartyInvoiceStateMachine.InvoiceAddedToPaymentsTrigger,
                invoiceAddedToPaymentBatchEvent.Id, invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey);

            var systemMessages = SystemMessageCollection.Empty();
            var currentInvoice = this.DataModel.InvoiceCollection.First(inv => inv.ThirdPartyInvoiceKey == invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey);

            var postTransactionCorrelationGuid = invoiceAddedToPaymentBatchEvent.Id;
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, postTransactionCorrelationGuid);
            metadata.Add("username", this.DataModel.UserName);
            var postTransactionCommand = new ProcessTransactionsForThirdPartyInvoicePaymentCommand(invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey);
            payThirdPartyInvoiceStateMachine.RecordRequestSent(postTransactionCommand.GetType(), postTransactionCorrelationGuid);
            systemMessages.Aggregate(financeDomainService.PerformCommand(postTransactionCommand, metadata));


            if (systemMessages.HasErrors)
            {
                currentInvoice.StepInProcess = PaymentProcessStep.PostingTransactionFailed;
                payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Enqueue(systemMessages);
                payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Enqueue(invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey);
                payThirdPartyInvoiceStateMachine.RecordErrorResponseOrCommandFailed(postTransactionCorrelationGuid);
            }
            else
            {
                currentInvoice.StepInProcess = PaymentProcessStep.ReadyForArchiving;
            }

            if (!this.DataModel.InvoiceCollection.Any(inv => inv.StepInProcess == PaymentProcessStep.ReadyForPostingTransation))
            {
                payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(payThirdPartyInvoiceStateMachine.BatchAddedToPaymentsTrigger, invoiceAddedToPaymentBatchEvent.Id);
                var postedInvoices = this.DataModel.InvoiceCollection.Where(inv => inv.StepInProcess == PaymentProcessStep.ReadyForArchiving).Count();
                var receivedEvents = payThirdPartyInvoiceStateMachine.GetStateHistoryCount(InvoicePaymentProcessState.InvoiceTransactionPosted);
                if (postedInvoices == receivedEvents && postedInvoices > 0)
                {
                    CreateCatsFile();
                }
            }
        }
    }
}