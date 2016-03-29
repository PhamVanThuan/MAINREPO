using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FinanceDomain.Commands;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_getting_batch_reference_fails : PayThirdPartyInvoiceProcessSpec
    {
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
        private static int batchReference;
        public static List<PayThirdPartyInvoiceModel> invoices;
        public static Task<IDomainProcessStartResult> startPayAttorneyInvoiceProcessTask;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            case1Instance = 1000000000000000001;
            case2Instance = 1000000000000000101;
            case3Instance = 1000000000000000104;
            case4Instance = 1000000000000000131;

            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                   new PayThirdPartyInvoiceModel(1, case1Instance, 2321422, "SAHL-2015/07/359",  PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(2, case2Instance, 4077922, "SAHL-2015/07/335",  PaymentProcessStep.PreparingWorkflowCase )
                                ,  new PayThirdPartyInvoiceModel(3, case3Instance, 4077835, "SAHL-2015/07/312",  PaymentProcessStep.PreparingWorkflowCase)
                                ,  new PayThirdPartyInvoiceModel(4, case4Instance, 4080071, "SAHL-2015/07/367",  PaymentProcessStep.PreparingWorkflowCase)
                            };

            batchReference = 15;
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

            var serviceErrorMessage = new List<ISystemMessage> { new SystemMessage("Internal Server Error", SystemMessageSeverityEnum.Error) };
            catsService.PerformQuery(Arg.Is<GetNewThirdPartyPaymentBatchReferenceQuery>(y =>
               y.BatchType == Core.BusinessModel.Enums.CATSPaymentBatchType.ThirdPartyInvoice)).Returns(new SystemMessageCollection(serviceErrorMessage));

            var messagesQueue = Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(messagesQueue);

            payThirdPartyInvoiceStateMachine.BatchReference.Returns(batchReference);
            dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");
        };

        private Because of = () =>
        {
            startPayAttorneyInvoiceProcessTask = payAttorneyInvoiceDomainProcess.Start(dataModel, string.Empty);
        };

        private It should_execute_registered_rules = () =>
        {
            domainRuleManager.Received().ExecuteRules(Arg.Any<ISystemMessageCollection>(), dataModel);
        };


        private It should_attempt_to_get_batch_reference = () =>
        {
            catsService.Received().PerformQuery(Arg.Is<GetNewThirdPartyPaymentBatchReferenceQuery>(m =>
               m.BatchType == Core.BusinessModel.Enums.CATSPaymentBatchType.ThirdPartyInvoice));
        };

        private It should_fail_to_complete_task = () =>
        {
            startPayAttorneyInvoiceProcessTask.IsFaulted.ShouldBeTrue();
        };

        private It should_throw_an_exception = () =>
        {
            startPayAttorneyInvoiceProcessTask.Exception.InnerException.Message.ShouldContain("Failed to acquire a batch reference");
        };

        private It should_not_set_batch_reference = () =>
        {
            payThirdPartyInvoiceStateMachine.DidNotReceive().FireStateMachineTriggerWithKey(
                  Arg.Is<InvoicePaymentStateTransitionTrigger>(m => m == InvoicePaymentStateTransitionTrigger.BatchReferenceReceived)
                , Arg.Is<Guid>(m => m != Guid.Empty)
                , Arg.Is<int>(m => m == batchReference)
             );
        };

        private It should_not_move_cases_for_all_invoices_in_batch_to_ready_for_processing_workflow_state = () =>
        {
            x2WorkflowManager.DidNotReceive().ProcessThirdPartyInvoicePayment(
                  Arg.Is<long>(m => dataModel.InvoiceCollection.Select(i => i.InstanceId).Contains(m))
                , Arg.Is<int>(m => dataModel.InvoiceCollection.Select(i => i.AccountNumber).Contains(m))
                , Arg.Is<int>(m => dataModel.InvoiceCollection.Select(i => i.ThirdPartyInvoiceKey).Contains(m))
                , Arg.Is<DomainProcessServiceRequestMetadata>(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString() &&
                    m.CommandCorrelationId != Guid.Empty)
            );
        };

        private It should_not_create_payment_batch_of_given_invoices_using_reference = () =>
        {
            financeDomainService.DidNotReceive().PerformCommand(
                Arg.Is<AddThirdPartyInvoiceToPaymentBatchCommand>(m => m.CATSPaymentBatchKey == batchReference)
              , Arg.Is<IServiceRequestMetadata>(m => m != null)
             );
        };
    }
}