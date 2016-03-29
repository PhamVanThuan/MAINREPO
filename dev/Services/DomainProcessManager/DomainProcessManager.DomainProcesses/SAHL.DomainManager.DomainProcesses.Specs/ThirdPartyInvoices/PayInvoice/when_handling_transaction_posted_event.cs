using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_handling_transaction_posted_event : PayThirdPartyInvoiceProcessSpec
    {
        private static TransactionsProcessedForThirdPartyInvoicePaymentEvent invoiceTransactionPostedEvent;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static int batchReference;
        public static List<PayThirdPartyInvoiceModel> invoices;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            batchReference = 15;
            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                   new PayThirdPartyInvoiceModel(18377, 1000000000000000001, 1399640, "SAHL-2015/07/359", PaymentProcessStep.ReadyForPostingTransation)
                                ,  new PayThirdPartyInvoiceModel(18378, 1000000000000000101, 4077922, "SAHL-2015/07/335", PaymentProcessStep.ReadyForPostingTransation )
                                ,  new PayThirdPartyInvoiceModel(18379, 1000000000000000104, 4077835, "SAHL-2015/07/312", PaymentProcessStep.ReadyForPostingTransation)
                                ,  new PayThirdPartyInvoiceModel(18380, 1000000000000000131, 4080071, "SAHL-2015/07/367", PaymentProcessStep.ReadyForPostingTransation)
                            };

            payAttorneyInvoiceDomainProcess = new PayAttorneyInvoiceProcess(
                  payThirdPartyInvoiceStateMachine
                , financeDomainService
                , combGuidGenerator
                , catsService
                , x2WorkflowManager
                , communicationManager
                , rawLogger
                , loggerSource
                , loggerAppSource
                , domainRuleManager
            );

            dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");
            payAttorneyInvoiceDomainProcess.SetDataModel(dataModel);

            invoiceTransactionPostedEvent = new TransactionsProcessedForThirdPartyInvoicePaymentEvent(
                  10023589
                , 4080071
                , 18380
                , 566.89M
                , DateTime.Now
                , "SAHL-Ref"
                , "SAHL\\BCUser"
             );

            payThirdPartyInvoiceStateMachine.BatchReference.Returns(batchReference);

            var invoiceTransactionPostedTrigger = new Stateless.StateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                .TriggerWithParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent);
            payThirdPartyInvoiceStateMachine.InvoiceTransactionPostedTrigger.Returns(invoiceTransactionPostedTrigger);

        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(invoiceTransactionPostedEvent, metadata);
        };

        private It should_track_posting_of_transaction = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTriggerWithKey(
                   Arg.Is<SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                    .TriggerWithParameters<Guid, int>>(t => t.Trigger == InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent)
                , Arg.Is<Guid>(m => m.Equals(invoiceTransactionPostedEvent.Id))
                , Arg.Is<int>(m => m == invoiceTransactionPostedEvent.ThirdPartyInvoiceKey)
            );
        };

        private It should_update_invoice_process_step = () =>
        {
            payAttorneyInvoiceDomainProcess.DataModel.InvoiceCollection
                .Where(inv => inv.StepInProcess == PaymentProcessStep.ReadyForArchiving)
                .Count()
                .ShouldEqual(1);
        };

        private It should_not_make_a_batch_payment = () =>
        {
            catsService.DidNotReceive().PerformCommand(
                  Arg.Is<ProcessCATSPaymentBatchCommand>(m => m.CATSPaymentBatchKey == batchReference)
                , Arg.Is<IServiceRequestMetadata>(m => m != null)
             );
        };
    }
}