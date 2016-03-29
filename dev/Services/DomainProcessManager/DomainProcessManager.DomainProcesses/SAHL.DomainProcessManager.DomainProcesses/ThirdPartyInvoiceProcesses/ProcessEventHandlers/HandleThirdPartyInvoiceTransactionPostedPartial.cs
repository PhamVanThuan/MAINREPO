using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public abstract partial class PayThirdPartyInvoiceDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<TransactionsProcessedForThirdPartyInvoicePaymentEvent>
        where T : PayThirdPartyInvoiceProcessModel
    {
        public void Handle(TransactionsProcessedForThirdPartyInvoicePaymentEvent invoiceTransactionPostedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            payThirdPartyInvoiceStateMachine.FireStateMachineTriggerWithKey(payThirdPartyInvoiceStateMachine.InvoiceTransactionPostedTrigger, invoiceTransactionPostedEvent.Id, invoiceTransactionPostedEvent.ThirdPartyInvoiceKey);

            var systemMessages = SystemMessageCollection.Empty();
            var currentInvoice = this.DataModel.InvoiceCollection.First(inv => inv.ThirdPartyInvoiceKey == invoiceTransactionPostedEvent.ThirdPartyInvoiceKey);
            currentInvoice.StepInProcess = PaymentProcessStep.ReadyForArchiving;

            if (!this.DataModel.InvoiceCollection.Any(inv => inv.StepInProcess == PaymentProcessStep.ReadyForPostingTransation))
            {
                var postedInvoices = this.DataModel.InvoiceCollection.Where(inv => inv.StepInProcess == PaymentProcessStep.ReadyForArchiving).Count();
                var receivedEvents = payThirdPartyInvoiceStateMachine.GetStateHistoryCount(InvoicePaymentProcessState.InvoiceTransactionPosted);
                if (postedInvoices == receivedEvents)
                {
                    CreateCatsFile();
                }
            }
        }
    }
}