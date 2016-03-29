using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_handling_last_transaction_posted_event_and_invoices_get_stuck : PayThirdPartyInvoiceProcessSpec
    {
        private static TransactionsProcessedForThirdPartyInvoicePaymentEvent invoiceTransactionPostedEvent;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static int batchReference;
        public static List<PayThirdPartyInvoiceModel> invoices;
        private static ConcurrentQueue<int> stuckInvoiceQueue;
        private static ConcurrentQueue<ISystemMessageCollection> systemMessagesQueue;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            batchReference = 15;
            stuckInvoiceQueue = new ConcurrentQueue<int>();
            systemMessagesQueue = Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();

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
            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(stuckInvoiceQueue);
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(systemMessagesQueue);

            var systemMessages = new List<ISystemMessage> { new SystemMessage("Internal Server Error", SystemMessageSeverityEnum.Error) };
            catsService.PerformCommand(
                  Arg.Is<ProcessCATSPaymentBatchCommand>(m => m.CATSPaymentBatchKey == batchReference)
                , Arg.Is<IServiceRequestMetadata>(m => m != null)
             ).Returns(new SystemMessageCollection(systemMessages));


            var invoicePostedTrigger = new Stateless.StateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent);
            payThirdPartyInvoiceStateMachine.InvoiceTransactionPostedTrigger.Returns(invoicePostedTrigger);
            payThirdPartyInvoiceStateMachine.GetStateHistoryCount(InvoicePaymentProcessState.InvoiceTransactionPosted).Returns(invoices.Count);

        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(invoiceTransactionPostedEvent, metadata);
        };

        private It should_track_posting_of_invoice_transaction = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTriggerWithKey(
                  Arg.Is<SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int>>(t => t.Trigger == InvoicePaymentStateTransitionTrigger.InvoiceTransactionPostedEvent)
                , Arg.Is<Guid>(m => m == invoiceTransactionPostedEvent.Id)
                , Arg.Is<int>(m => m == invoiceTransactionPostedEvent.ThirdPartyInvoiceKey));
        };

        private It should_process_a_payment_batch = () =>
        {
            catsService.Received().PerformCommand(
                  Arg.Is<ProcessCATSPaymentBatchCommand>(m => m.CATSPaymentBatchKey == batchReference)
                , Arg.Is<IServiceRequestMetadata>(m => m != null)
             );
        };

        private It should_mark_all_invoices_as_stuck = () =>
        {
            invoices.All(y => y.StepInProcess == PaymentProcessStep.PaymentBatchFailed).ShouldBeTrue();
        };

        private It should_record_the_response_error = () =>
        {
            payThirdPartyInvoiceStateMachine.RecordErrorResponseOrCommandFailed(Arg.Is<Guid>(g => g != null));
        };

        private It should_track_stuck_invoices = () =>
        {
            stuckInvoiceQueue.All(invoices.Select(y => y.ThirdPartyInvoiceKey).Contains);
            invoices.Count.ShouldEqual(stuckInvoiceQueue.Count);
        };
    }
}