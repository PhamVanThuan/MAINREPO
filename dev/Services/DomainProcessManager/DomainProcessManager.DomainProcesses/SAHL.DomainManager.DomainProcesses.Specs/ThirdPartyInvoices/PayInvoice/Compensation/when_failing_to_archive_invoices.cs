using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice.Compensation
{
    public class when_failing_to_archive_invoices : PayThirdPartyInvoiceProcessSpec
    {
        private static ConcurrentQueue<int> stuckInvoiceQueue;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        public static List<PayThirdPartyInvoiceModel> invoices;
        private static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                    new PayThirdPartyInvoiceModel(18377, 1000000000000000001, 1399640, "SAHL-2015/07/359", PaymentProcessStep.ReadyForArchiving)
                                ,  new PayThirdPartyInvoiceModel(18378, 1000000000000000101, 4077922, "SAHL-2015/07/335", PaymentProcessStep.ReadyForArchiving )
                                ,  new PayThirdPartyInvoiceModel(18379, 1000000000000000104, 4077835, "SAHL-2015/07/312", PaymentProcessStep.ReadyForArchiving)
                            };
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

            dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");
            payAttorneyInvoiceDomainProcess.SetDataModel(dataModel);

            stuckInvoiceQueue = new ConcurrentQueue<int>();
            Parallel.ForEach(dataModel.InvoiceCollection, (invoice) =>
            {
                invoice.StepInProcess = PaymentProcessStep.ArchivingFailed;
                stuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
            });
            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(stuckInvoiceQueue);

            var systemMessagesQueue = NSubstitute.Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(systemMessagesQueue);
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.CompensateStuckInvoices();
        };

        private It should_request_for_manual_archive = () =>
        {
            communicationManager.Received().SendOperatorRequestForManualArchive(
                Arg.Is<List<string>>(m => m.All(dataModel.InvoiceCollection.Select(z => z.SAHLReference).Contains))
            );
        };
    }
}