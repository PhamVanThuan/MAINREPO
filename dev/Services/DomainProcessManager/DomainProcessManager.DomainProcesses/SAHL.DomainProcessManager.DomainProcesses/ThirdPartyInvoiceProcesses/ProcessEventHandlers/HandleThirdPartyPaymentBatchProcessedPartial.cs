using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Events;
using System.Linq;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public abstract partial class PayThirdPartyInvoiceDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<CATSPaymentBatchProcessedEvent>
        where T : PayThirdPartyInvoiceProcessModel
    {
        public void Handle(CATSPaymentBatchProcessedEvent paymentBatchProcessedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            payThirdPartyInvoiceStateMachine.FireStateMachineTriggerWithKey(payThirdPartyInvoiceStateMachine.PaymentBatchProcessedTrigger, paymentBatchProcessedEvent.Id, paymentBatchProcessedEvent.BatchReferenceNumber);

            var systemMessages = SystemMessageCollection.Empty();
            Parallel.ForEach(this.DataModel.InvoiceCollection.Where(y => y.StepInProcess == PaymentProcessStep.ReadyForArchiving), (invoice) =>
            {
                var archiveInvoiceCorrelationId = this.combGuidGenerator.Generate();
                var serviceMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, archiveInvoiceCorrelationId);
                systemMessages.Aggregate(x2WorkflowManager.ArchiveThirdPartyInvoice(
                      invoice.InstanceId
                    , invoice.AccountNumber
                    , invoice.ThirdPartyInvoiceKey
                    , serviceMetadata
                ));

                invoice.StepInProcess = PaymentProcessStep.Archived;
                if (systemMessages.HasErrors)
                {
                    payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Enqueue(systemMessages);
                    payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
                    invoice.StepInProcess = PaymentProcessStep.ArchivingFailed;
                }
            });
            var notifyCatsPaymentBatchCommandCorrelationId = this.combGuidGenerator.Generate();
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, notifyCatsPaymentBatchCommandCorrelationId);
            var notifyCatsPaymentBatchCommand = new NotifyCATSPaymentBatchRecipientsCommand(payThirdPartyInvoiceStateMachine.BatchReference);
            payThirdPartyInvoiceStateMachine.RecordRequestSent(notifyCatsPaymentBatchCommand.GetType(), notifyCatsPaymentBatchCommandCorrelationId);
            systemMessages.Aggregate(catsService.PerformCommand(notifyCatsPaymentBatchCommand, metadata));
            if (systemMessages.HasErrors || payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Any())
            {
                payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Enqueue(systemMessages);
                payThirdPartyInvoiceStateMachine.RecordErrorResponseOrCommandFailed(notifyCatsPaymentBatchCommandCorrelationId);
                payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(payThirdPartyInvoiceStateMachine.PartialCompletionConfirmedTrigger, combGuidGenerator.Generate());
            }
            else
            {
                payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(payThirdPartyInvoiceStateMachine.BatchArchivedTrigger, combGuidGenerator.Generate());
                payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(payThirdPartyInvoiceStateMachine.CompletionConfirmedTrigger, combGuidGenerator.Generate());
            }

        }
    }
}