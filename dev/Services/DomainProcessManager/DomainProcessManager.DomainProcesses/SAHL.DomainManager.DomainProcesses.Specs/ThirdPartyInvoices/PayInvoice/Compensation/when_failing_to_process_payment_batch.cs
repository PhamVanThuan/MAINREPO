using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands;


using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice.Compensation
{
    public class when_failing_to_process_payment_batch : PayThirdPartyInvoiceProcessSpec
    {
        private static PayThirdPartyInvoiceProcessModel dataModel;
        private static int batchReference;
        private static List<PayThirdPartyInvoiceModel> invoices;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        { 
            domainRuleManager= An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            batchReference = 15;
            invoices = new List<PayThirdPartyInvoiceModel> {
                   new PayThirdPartyInvoiceModel(18378, 1000000000000000101, 4077922, "SAHL-2015/07/359", PaymentProcessStep.ReadyForArchiving )
                ,  new PayThirdPartyInvoiceModel(18377, 1000000000000000001, 1399640, "SAHL-2015/07/335", PaymentProcessStep.ReadyForArchiving)
                ,  new PayThirdPartyInvoiceModel(18379, 1000000000000000104, 4077835, "SAHL-2015/07/312", PaymentProcessStep.ReadyForArchiving)
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
                ,domainRuleManager
            );

            dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");

            payAttorneyInvoiceDomainProcess.SetDataModel(dataModel);

            ConcurrentQueue<int> stuckInvoiceQueue = new ConcurrentQueue<int>();
            Parallel.ForEach(dataModel.InvoiceCollection, (invoice) =>
            {
                invoice.StepInProcess = PaymentProcessStep.PaymentBatchFailed;
                stuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
            });
            payThirdPartyInvoiceStateMachine.WhenToldTo(y => y.StuckInvoiceQueue).Return(stuckInvoiceQueue);

            payThirdPartyInvoiceStateMachine.WhenToldTo(y => y.BatchReference).Return(batchReference);

            var systemMessagesQueue = NSubstitute.Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();
            payThirdPartyInvoiceStateMachine.WhenToldTo(y => y.SystemMessagesQueue).Return(systemMessagesQueue);
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.CompensateStuckInvoices();
        };

        private It should_reverse_all_posted_transactions = () =>
        {
            financeDomainService.Received(invoices.Count).PerformCommand(
                Arg.Is<ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand>(m =>
                   invoices.Select(i => i.ThirdPartyInvoiceKey).Contains(m.ThirdPartyInvoiceKey)
                )
              , Arg.Is<IServiceRequestMetadata>(m => m != null)
             );
        };

        private It should_remove_all_invoices_from_payment_batch = () =>
        {
            financeDomainService.Received(invoices.Count).PerformCommand(
               Arg.Is<RemoveThirdPartyInvoiceFromPaymentBatchCommand>(m =>
                   m.CATSPaymentBatchKey == batchReference &&
                  invoices.Select(i => i.ThirdPartyInvoiceKey).Contains(m.ThirdPartyInvoiceKey)
               )
             , Arg.Is<IServiceRequestMetadata>(m => m != null)
            );
        };

        private It should_return_failing_invoices_workflow_cases_to_payment_approved = () =>
        {
            x2WorkflowManager.Received(invoices.Count).ReversePayment(
                  Arg.Is<long>(m => invoices.Select(i => i.InstanceId).Contains(m))
                , Arg.Is<int>(m => invoices.Select(i => i.AccountNumber).Contains(m))
                , Arg.Is<int>(m => invoices.Select(i => i.ThirdPartyInvoiceKey).Contains(m))
                , Arg.Is<DomainProcessServiceRequestMetadata>(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == payAttorneyInvoiceDomainProcess.DomainProcessId.ToString()
             ));
        };
    }
}