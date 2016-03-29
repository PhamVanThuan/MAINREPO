using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;

using System;
using System.Collections.Generic;
using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    [Ignore("Crazy NSubstitute bug")]
    public class when_handling_last_transaction_posted_event : PayThirdPartyInvoiceProcessSpec
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
                                   new PayThirdPartyInvoiceModel(18377, 1000000000000000001, 1399640, "SAHL-2015/07/359", PaymentProcessStep.ReadyForArchiving)
                                ,  new PayThirdPartyInvoiceModel(18378, 1000000000000000101, 4077922, "SAHL-2015/07/335", PaymentProcessStep.ReadyForArchiving )
                                ,  new PayThirdPartyInvoiceModel(18379, 1000000000000000104, 4077835, "SAHL-2015/07/312", PaymentProcessStep.ReadyForArchiving)
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
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(invoiceTransactionPostedEvent, metadata);
        };

        private It should_track_posting_transactions_for_eligible_invoices = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTrigger(
                  Arg.Is<InvoicePaymentStateTransitionTrigger>(m => m == InvoicePaymentStateTransitionTrigger.PaymentBatchTransactionsPostedConfirmation)
                , Arg.Is<Guid>(m => m != Guid.Empty)
            );
        };

        private It should_process_a_payment_batch = () =>
        {
            catsService.Received().PerformCommand(
                  Arg.Is<ProcessCATSPaymentBatchCommand>(m => m.CATSPaymentBatchKey == batchReference)
                , Arg.Is<IServiceRequestMetadata>(m => m != null)
             );
        };
    }
}