using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_processing_transactions_returns_errors : PayThirdPartyInvoiceProcessSpec
    {
        private static ThirdPartyInvoiceAddedToBatchEvent invoiceAddedToPaymentBatchEvent;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
        public static List<PayThirdPartyInvoiceModel> invoices;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;
        private static ISystemMessageCollection errorMessages;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            case1Instance = 1000000000000000001;
            case2Instance = 1000000000000000101;
            case3Instance = 1000000000000000104;
            case4Instance = 1000000000000000131;

            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                   new PayThirdPartyInvoiceModel(1, case1Instance, 2321422, "SAHL-2015/07/359", PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(2, case2Instance, 4077922, "SAHL-2015/07/335", PaymentProcessStep.PreparingWorkflowCase )
                                ,  new PayThirdPartyInvoiceModel(3, case3Instance, 4077835, "SAHL-2015/07/312", PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(4, case4Instance, 4080071, "SAHL-2015/07/367", PaymentProcessStep.PreparingWorkflowCase)
                            };

            invoiceAddedToPaymentBatchEvent = new ThirdPartyInvoiceAddedToBatchEvent(DateTime.Now, 2, 3, 1300M);

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

            var invoiceAddedToPaymentTrigger = new Stateless.StateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.InvoiceAddedToPaymentEvent);
            payThirdPartyInvoiceStateMachine.InvoiceAddedToPaymentsTrigger.Returns(invoiceAddedToPaymentTrigger);

            errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("An internal error occured", SystemMessageSeverityEnum.Error));
            financeDomainService.PerformCommand(Arg.Is<ProcessTransactionsForThirdPartyInvoicePaymentCommand>(m =>
                m.ThirdPartyInvoiceKey == invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey)
                , Arg.Is<IServiceRequestMetadata>(m => ((DomainProcessServiceRequestMetadata)m).ContainsKey(CoreGlobals.DomainProcessIdName) &&
                     ((DomainProcessServiceRequestMetadata)m)[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString())
             ).Returns<ISystemMessageCollection>(errorMessages);

            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(new ConcurrentQueue<ISystemMessageCollection>());
            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(new ConcurrentQueue<int>());
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(invoiceAddedToPaymentBatchEvent, metadata);
        };

        private It should_track_addition_of_invoice_to_payment_batch = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTriggerWithKey(
                  Arg.Is<SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int>>(t => t.Trigger == InvoicePaymentStateTransitionTrigger.InvoiceAddedToPaymentEvent)
                , Arg.Is<Guid>(m => m == invoiceAddedToPaymentBatchEvent.Id)
                , Arg.Is<int>(m => m == invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey));
        };

        private It should_post_the_invoice_transaction = () =>
        {
            financeDomainService.Received().PerformCommand(Arg.Is<ProcessTransactionsForThirdPartyInvoicePaymentCommand>(m =>
                m.ThirdPartyInvoiceKey == invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey)
                , Arg.Is<IServiceRequestMetadata>(m => ((DomainProcessServiceRequestMetadata)m).ContainsKey(CoreGlobals.DomainProcessIdName) &&
                     ((DomainProcessServiceRequestMetadata)m)[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString())
             );
        };

        private It should_add_the_error_message_to_the_queue = () =>
        {
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.First().AllMessages.First().Message.ShouldEqual("An internal error occured");
        };


        private It should_add_the_invoice_to_the_stuck_queue = () =>
        {
            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.First().ShouldEqual(invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey);
        };

        private It should_record_the_error_response = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().RecordErrorResponseOrCommandFailed(Arg.Is<Guid>(g => g != null));

        };

        private It should_update_invoices_processing_step = () =>
        {
            payAttorneyInvoiceDomainProcess.DataModel.InvoiceCollection
                .First(inv => inv.ThirdPartyInvoiceKey == invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey)
                .StepInProcess.ShouldEqual(PaymentProcessStep.PostingTransactionFailed);
        };
    }
}