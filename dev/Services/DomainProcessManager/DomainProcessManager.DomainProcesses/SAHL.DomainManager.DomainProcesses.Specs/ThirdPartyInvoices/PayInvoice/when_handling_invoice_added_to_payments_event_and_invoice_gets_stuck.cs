using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    [Ignore("Maybe causing side effects to next spec")]
    public class when_handling_invoice_added_to_payments_event_and_invoice_gets_stuck : PayThirdPartyInvoiceProcessSpec
    {
        private static ThirdPartyInvoiceAddedToBatchEvent invoiceAddedToPaymentBatchEvent;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
        public static List<PayThirdPartyInvoiceModel> invoices;
        public static PayThirdPartyInvoiceModel stuckInvoice;
        private static ConcurrentQueue<int> stuckInvoiceQueue;
        private static ConcurrentQueue<ISystemMessageCollection> systemMessagesQueue;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            case1Instance = 1000000000000000001;
            case2Instance = 1000000000000000101;
            case3Instance = 1000000000000000104;
            case4Instance = 1000000000000000131;

            stuckInvoiceQueue = Substitute.For<ConcurrentQueue<int>>();
            systemMessagesQueue = new ConcurrentQueue<ISystemMessageCollection>();

            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                   new PayThirdPartyInvoiceModel(1, case1Instance, 2321422, "SAHL-2015/07/359", PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(2, case2Instance, 4077922, "SAHL-2015/07/335", PaymentProcessStep.PreparingWorkflowCase )
                                ,  new PayThirdPartyInvoiceModel(3, case3Instance, 4077835, "SAHL-2015/07/312", PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(4, case4Instance, 4080071, "SAHL-2015/07/367", PaymentProcessStep.PreparingWorkflowCase)
                            };

            invoiceAddedToPaymentBatchEvent = new ThirdPartyInvoiceAddedToBatchEvent(DateTime.Now, 2, 4, 1300M);

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

            stuckInvoice = invoices.Single(i => i.ThirdPartyInvoiceKey == invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey);

            var postTransactionErrorMessage = new List<ISystemMessage> { new SystemMessage("Internal Server Error", SystemMessageSeverityEnum.Error) };
            financeDomainService.PerformCommand(
                  Arg.Is<ProcessTransactionsForThirdPartyInvoicePaymentCommand>(m =>
                  m.ThirdPartyInvoiceKey == invoiceAddedToPaymentBatchEvent.ThirdPartyInvoiceKey
                  )
                , Arg.Is<IServiceRequestMetadata>(m => m != null)
            ).Returns(new SystemMessageCollection(postTransactionErrorMessage));

            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(stuckInvoiceQueue);
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(systemMessagesQueue);
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(invoiceAddedToPaymentBatchEvent, metadata);
        };

        private It should_track_addition_of_invoice_to_payment_batch = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTrigger(
                  Arg.Is<InvoicePaymentStateTransitionTrigger>(m => m == InvoicePaymentStateTransitionTrigger.InvoiceAddedToPaymentEvent)
                , Arg.Is<Guid>(m => m == invoiceAddedToPaymentBatchEvent.Id));
        };

        private It should_mark_stuck_invoice = () =>
        {
            stuckInvoice.StepInProcess.ShouldEqual(PaymentProcessStep.PostingTransactionFailed);
        };

        private It should_track_stuck_invoice = () =>
        {
            stuckInvoiceQueue.Received().Enqueue(Arg.Is<int>(z => z == stuckInvoice.ThirdPartyInvoiceKey));
        };
    }
}