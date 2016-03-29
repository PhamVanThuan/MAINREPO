using Machine.Fakes;
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
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_processing_payment_batch_end_in_some_stuck_invoices : PayThirdPartyInvoiceProcessSpec
    {
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
        private static Guid x2PayInvoiceCorrelationId;
        private static int batchReference;
        private static int numberOfStuckInvoices;
        public static List<PayThirdPartyInvoiceModel> invoices;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
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

            var fixedVerifiableGuid = Guid.NewGuid();
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(fixedVerifiableGuid);

            case1Instance = 1000000000000000001;
            case2Instance = 1000000000000000101;
            case3Instance = 1000000000000000104;
            case4Instance = 1000000000000000131;

            x2PayInvoiceCorrelationId = combGuidGenerator.Generate();

            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                   new PayThirdPartyInvoiceModel(1, case1Instance, 2321422, "SAHL-2015/07/359", PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(2, case2Instance, 4077922, "SAHL-2015/07/335", PaymentProcessStep.PreparingWorkflowCase )
                                ,  new PayThirdPartyInvoiceModel(3, case3Instance, 4077835, "SAHL-2015/07/312", PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(4, case4Instance, 4080071, "SAHL-2015/07/367", PaymentProcessStep.PreparingWorkflowCase)
                            };

            dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");

            batchReference = 15;

            numberOfStuckInvoices = 2;

            var systemMessages = new List<ISystemMessage>() { new SystemMessage("No Route error", SystemMessageSeverityEnum.Error) };
            x2WorkflowManager.ProcessThirdPartyInvoicePayment(
                  Arg.Is<long>(m => m == case1Instance || m == case2Instance)
                , Arg.Is<int>(m => m > 0)
                , Arg.Is<int>(m => m > 0)
                , Arg.Is<DomainProcessServiceRequestMetadata>(m => m.ContainsKey(CoreGlobals.DomainProcessIdName))
             ).Returns(new SystemMessageCollection(systemMessages));

            var queryResult = new GetNewThirdPartyPaymentBatchReferenceQueryResult();
            queryResult.BatchKey = batchReference;
            ServiceQueryResult<GetNewThirdPartyPaymentBatchReferenceQueryResult> serviceQueryResult =
                new ServiceQueryResult<GetNewThirdPartyPaymentBatchReferenceQueryResult>(new GetNewThirdPartyPaymentBatchReferenceQueryResult[] { queryResult });
            catsService.PerformQuery(Arg.Do<GetNewThirdPartyPaymentBatchReferenceQuery>(query => query.Result = serviceQueryResult))
              .Returns(SystemMessageCollection.Empty());

            var messagesQueue = Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(messagesQueue);

            var stuckInvoiceQueue = Substitute.For<ConcurrentQueue<int>>();
            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(stuckInvoiceQueue);

            payThirdPartyInvoiceStateMachine.BatchReference.Returns(batchReference);

            var batchReceivedTrigger = new Stateless.StateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int>(InvoicePaymentStateTransitionTrigger.BatchReferenceReceived);
            payThirdPartyInvoiceStateMachine.BatchReferenceSetTrigger.Returns(batchReceivedTrigger);
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Start(dataModel, string.Empty);
        };

        private It should_get_a_batch_reference = () =>
        {
            catsService.Received().PerformQuery(Arg.Is<GetNewThirdPartyPaymentBatchReferenceQuery>(m =>
            m.BatchType == Core.BusinessModel.Enums.CATSPaymentBatchType.ThirdPartyInvoice));
        };

        private It should_track_setting_of_batch_reference = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().FireStateMachineTriggerWithKey(
                  Arg.Is<SerialisationFriendlyStateMachine<InvoicePaymentProcessState, InvoicePaymentStateTransitionTrigger>.TriggerWithParameters<Guid, int>>(t => t.Trigger == InvoicePaymentStateTransitionTrigger.BatchReferenceReceived)
                , Arg.Is<Guid>(m => m != Guid.Empty)
                , Arg.Is<int>(m => m == batchReference)
             );
        };

        private It should_move_cases_for_all_invoices_in_batch_to_ready_for_processing_workflow_state = () =>
        {
            x2WorkflowManager.Received(dataModel.InvoiceCollection.Count).ProcessThirdPartyInvoicePayment(
                  Arg.Is<long>(m => dataModel.InvoiceCollection.Select(i => i.InstanceId).Contains(m))
                , Arg.Is<int>(m => dataModel.InvoiceCollection.Select(i => i.AccountNumber).Contains(m))
                , Arg.Is<int>(m => dataModel.InvoiceCollection.Select(i => i.ThirdPartyInvoiceKey).Contains(m))
                , Arg.Is<DomainProcessServiceRequestMetadata>(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString() &&
                    m.CommandCorrelationId == x2PayInvoiceCorrelationId)
            );
        };

        private It should_track_stuck_invoices_and_the_correct_process_step_stuck_at = () =>
        {
            invoices.Where(y => y.StepInProcess == PaymentProcessStep.PreparingWorkflowCaseFailed).Count().ShouldEqual(numberOfStuckInvoices);
        };

        private It should_create_payment_batch_excluding_stuck_invoices_using_reference = () =>
        {
            financeDomainService.Received(dataModel.InvoiceCollection.Count - numberOfStuckInvoices).PerformCommand(
                  Arg.Is<AddThirdPartyInvoiceToPaymentBatchCommand>(m =>
                     dataModel.InvoiceCollection.Select(i => i.ThirdPartyInvoiceKey).Contains(m.ThirdPartyInvoiceKey)
                     && m.CATSPaymentBatchKey == batchReference)
                , Arg.Is<DomainProcessServiceRequestMetadata>(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString())
            );
        };

        private It should_mark_non_stuck_invoices_as_ready_posting_transaction = () =>
        {
            dataModel.InvoiceCollection.Where(inv => inv.StepInProcess == PaymentProcessStep.ReadyForPostingTransation).Count()
                .ShouldEqual(dataModel.InvoiceCollection.Count - numberOfStuckInvoices);
        };
    }
}