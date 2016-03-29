using SAHL.Core.DomainProcess;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System.Linq;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public abstract partial class PayThirdPartyInvoiceDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<TransactionsProcessedForThirdPartyInvoicePaymentEvent>
        where T : PayThirdPartyInvoiceProcessModel
    {
        public void CreateCatsFile()
        {

            var systemMessages = SystemMessageCollection.Empty();

            if (!this.DataModel.InvoiceCollection.Any(inv => inv.StepInProcess == PaymentProcessStep.ReadyForPostingTransation))
            {
                CompensateStuckInvoices();

                var processPaymentBatchCorrelationId = this.combGuidGenerator.Generate();
                var serviceMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, processPaymentBatchCorrelationId);
                payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(payThirdPartyInvoiceStateMachine.BatchTransactionsPostedTrigger, processPaymentBatchCorrelationId);

                var processBatchPaymentCommand = new ProcessCATSPaymentBatchCommand(payThirdPartyInvoiceStateMachine.BatchReference);
                payThirdPartyInvoiceStateMachine.RecordRequestSent(processBatchPaymentCommand.GetType(), processPaymentBatchCorrelationId);
                systemMessages.Aggregate(catsService.PerformCommand(processBatchPaymentCommand, serviceMetadata));

                if (systemMessages.HasErrors)
                {
                    payThirdPartyInvoiceStateMachine.RecordErrorResponseOrCommandFailed(processPaymentBatchCorrelationId);
                    payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Enqueue(systemMessages);
                    Parallel.ForEach(this.DataModel.InvoiceCollection.Where(y => y.StepInProcess == PaymentProcessStep.ReadyForArchiving), (invoice) =>
                    {
                        invoice.StepInProcess = PaymentProcessStep.PaymentBatchFailed;
                        payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
                    });
                    CompensateStuckInvoices();
                }
            }
        }
    }
}