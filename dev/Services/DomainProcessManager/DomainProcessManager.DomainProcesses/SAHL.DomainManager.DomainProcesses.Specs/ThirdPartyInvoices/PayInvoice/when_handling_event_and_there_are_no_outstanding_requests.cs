using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;


using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_handling_event_and_there_are_no_outstanding_requests : PayThirdPartyInvoiceProcessSpec
    {
        private static ConcurrentQueue<int> stuckInvoiceQueue;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
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

            payThirdPartyInvoiceStateMachine.AllRequestsBeenServiced().Returns(true);

            dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");
            payAttorneyInvoiceDomainProcess.SetDataModel(dataModel);
            payAttorneyInvoiceDomainProcess.ProcessState = payThirdPartyInvoiceStateMachine;
            stuckInvoiceQueue = new ConcurrentQueue<int>();
            Parallel.ForEach(dataModel.InvoiceCollection.Where(y => y.ThirdPartyInvoiceKey == 3 || y.ThirdPartyInvoiceKey == 4), (invoice) =>
            {
                invoice.StepInProcess = PaymentProcessStep.ArchivingFailed;
                stuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
            });
            payThirdPartyInvoiceStateMachine.WhenToldTo(y => y.StuckInvoiceQueue).Return(stuckInvoiceQueue);

            var systemMessagesQueue = Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(systemMessagesQueue);
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.HandledEvent(metadata);
        };

        private It should_compensate_for_stuck_cases = () =>
        {
            communicationManager.Received().SendOperatorRequestForManualArchive(Arg.Is<List<string>>(m =>
                m.All(dataModel.InvoiceCollection.Select(z => z.SAHLReference).Contains)));
        };

        private It should_clear_the_stuck_invoice_queue = () =>
        {
            payThirdPartyInvoiceStateMachine.Received().ClearStuckInvoiceQueue();
        };
    }
}