using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Events;
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
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_handling_event_exception : PayThirdPartyInvoiceProcessSpec
    {
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
        public static List<PayThirdPartyInvoiceModel> invoices;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;
        private static IEvent @event;
        private static Exception runtimeException;

        private Establish context = () =>
        {
            @event = An<IEvent>();
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

            metadata = new DomainProcessServiceRequestMetadata(payAttorneyInvoiceDomainProcess.DomainProcessId, combGuidGenerator.Generate());
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

            runtimeException = new Exception("Something bad happened.");
            payThirdPartyInvoiceStateMachine.AllRequestsBeenServiced().Returns(true);
            var stuckInvoiceQueue = Substitute.For<ConcurrentQueue<int>>();
            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(stuckInvoiceQueue);
            var systemMessageQueue = Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(systemMessageQueue);


        };
        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.HandleException(@event, metadata, runtimeException);
        };

        private It should_record_response_or_event_received = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().RecordResponseOrEventReceived(Guid.Parse(metadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        };

        private It should_check_if_all_requests_were_serviced = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().AllRequestsBeenServiced();
        };

        private It should_clear_the_stuck_invoice_queue = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().ClearStuckInvoiceQueue();
        };
    }
}
