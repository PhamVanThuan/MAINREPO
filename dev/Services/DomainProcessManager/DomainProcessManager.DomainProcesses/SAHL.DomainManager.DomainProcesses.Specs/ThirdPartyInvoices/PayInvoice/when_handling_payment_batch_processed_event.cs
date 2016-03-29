using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_handling_payment_batch_processed_event : PayThirdPartyInvoiceProcessSpec
    {
        private static CATSPaymentBatchProcessedEvent paymentBatchProcessedEvent;
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
        private static int batchReference;
        private static CATSPaymentBatchTypeDataModel thirdPartyCATSPaymentBatch;
        public static List<PayThirdPartyInvoiceModel> invoices;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            thirdPartyCATSPaymentBatch = new CATSPaymentBatchTypeDataModel("ThirdpartyInvoice", "SHL04", "SSVS", (int)CATsEnvironment.Live, 1);

            case1Instance = 1000000000000000001;
            case2Instance = 1000000000000000101;
            case3Instance = 1000000000000000104;
            case4Instance = 1000000000000000131;
            batchReference = 15;

            payThirdPartyInvoiceStateMachine.BatchReference.Returns(batchReference);

            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                   new PayThirdPartyInvoiceModel(18377, case1Instance, 1399640, "SAHL-2015/07/359", PaymentProcessStep.Archived)
                                ,  new PayThirdPartyInvoiceModel(18378, case2Instance, 4077922, "SAHL-2015/07/335", PaymentProcessStep.Archived )
                                ,  new PayThirdPartyInvoiceModel(18379, case3Instance, 4077835, "SAHL-2015/07/312", PaymentProcessStep.Archived)
                                ,  new PayThirdPartyInvoiceModel(18380, case4Instance, 4080071, "SAHL-2015/07/367", PaymentProcessStep.ReadyForArchiving)
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

            paymentBatchProcessedEvent = new CATSPaymentBatchProcessedEvent(DateTime.Now, batchReference);

            var paymentBatchProcessedEventTrigger = new Stateless.StateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                .TriggerWithParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.PaymentBatchProcessedEvent);
            payThirdPartyInvoiceStateMachine.PaymentBatchProcessedTrigger.Returns(paymentBatchProcessedEventTrigger);


            var archivedInvoiceTrigger = new Stateless.StateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                .TriggerWithParameters<Guid>(InvoicePaymentStateTransitionTrigger.PaymentBatchArchivedConfirmation);
            payThirdPartyInvoiceStateMachine.BatchArchivedTrigger.Returns(archivedInvoiceTrigger);

            var completionConfirmedTrigger = new Stateless.StateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                .TriggerWithParameters<Guid>(InvoicePaymentStateTransitionTrigger.CompletionConfirmed);
            payThirdPartyInvoiceStateMachine.CompletionConfirmedTrigger.Returns(completionConfirmedTrigger);

            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(new ConcurrentQueue<int>());
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(paymentBatchProcessedEvent, metadata);
        };

        private It should_track_completion_of_payment_batch_processing = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTriggerWithKey(
                  Arg.Is<SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                    .TriggerWithParameters<Guid, int>>(t => t.Trigger == InvoicePaymentStateTransitionTrigger.PaymentBatchProcessedEvent)
                , Arg.Is<Guid>(m => m != Guid.Empty)
                , Arg.Is<int>(m => m == paymentBatchProcessedEvent.BatchReferenceNumber)
            );
        };

        private It should_archive_all_workflow_cases_for_invoices_in_batch = () =>
        {
            x2WorkflowManager.Received().ArchiveThirdPartyInvoice(
                  Arg.Is<long>(m => m > 0)
                , Arg.Is<int>(m => invoices.Select(i => i.AccountNumber).Contains(m))
                , Arg.Is<int>(m => invoices.Select(i => i.ThirdPartyInvoiceKey).Contains(m))
                , Arg.Is<DomainProcessServiceRequestMetadata>(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString())
            );
        };

        private It should_track_archived_invoices = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTrigger(
                  Arg.Is<SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                    .TriggerWithParameters<Guid>>(t => t.Trigger == InvoicePaymentStateTransitionTrigger.PaymentBatchArchivedConfirmation)
                , Arg.Is<Guid>(z => z != Guid.Empty)
            );
        };

        private It should_get_invoices_payments_recipients_notified = () =>
        {
            catsService.Received().PerformCommand(
                  Arg.Is<NotifyCATSPaymentBatchRecipientsCommand>(m => m.CATSPaymentBatchKey == batchReference)
                , Arg.Is<DomainProcessServiceRequestMetadata>(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString())
            );
        };

        private It should_successfully_complete = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTrigger(
                 Arg.Is<SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>
                    .TriggerWithParameters<Guid>>(t => t.Trigger == InvoicePaymentStateTransitionTrigger.CompletionConfirmed)
                , Arg.Is<Guid>(m => m != Guid.Empty)
            );
        };
    }
}