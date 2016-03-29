using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;


using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SAHL.Core.Rules;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice.Compensation
{
    public class when_there_are_error_messages : PayThirdPartyInvoiceProcessSpec
    {
        private static ConcurrentQueue<int> stuckInvoiceQueue;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        public static List<PayThirdPartyInvoiceModel> invoices;
        private static int batchReference;
        private static IEnumerable<PayThirdPartyInvoiceModel> failingInvoices;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            batchReference = 15;
            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                    new PayThirdPartyInvoiceModel(18377, 1000000000000000001, 1399640, "SAHL-2015/07/359", PaymentProcessStep.ReadyForArchiving)
                                ,  new PayThirdPartyInvoiceModel(18378, 1000000000000000101, 4077922, "SAHL-2015/07/335", PaymentProcessStep.ReadyForArchiving )
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
                , domainRuleManager
            );

            dataModel = new PayThirdPartyInvoiceProcessModel(invoices, "SAHL\\HaloUser");
            payAttorneyInvoiceDomainProcess.SetDataModel(dataModel);
            failingInvoices = dataModel.InvoiceCollection.Where(y => y.ThirdPartyInvoiceKey == 18378 || y.ThirdPartyInvoiceKey == 18379);

            var systemMessagesQueue = new ConcurrentQueue<ISystemMessageCollection>();
            stuckInvoiceQueue = new ConcurrentQueue<int>();
            Parallel.ForEach(failingInvoices, (invoice) =>
            {
                var errorMessage = string.Format("Failed to add invoice {0} to payment batch", invoice.SAHLReference);
                var errorMessages = new List<ISystemMessage> { new SystemMessage(errorMessage, SystemMessageSeverityEnum.Error) };

                var systemMessages = new SystemMessageCollection(errorMessages);
                systemMessagesQueue.Enqueue(systemMessages);
                invoice.StepInProcess = PaymentProcessStep.BatchingFailed;
                stuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
            });
            payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Returns(stuckInvoiceQueue);
            payThirdPartyInvoiceStateMachine.BatchReference.Returns(batchReference);

            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(systemMessagesQueue);
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.CompensateStuckInvoices();
        };

        private It should_log_error_messages = () =>
        {
            rawLogger.Received(failingInvoices.Count()).LogError(
                  Arg.Is<LogLevel>(m => m == loggerSource.LogLevel)
                , Arg.Is<string>(m => m.Equals(loggerSource.Name, System.StringComparison.Ordinal))
                , Arg.Is<string>(m => m.Equals(loggerAppSource.ApplicationName))
                , Arg.Is<string>(m => m != null)
                , Arg.Is<string>(m => m != null)
                , Arg.Is<string>(m =>
                    m.Contains(string.Format("Failed to add invoice {0} to payment batch", failingInvoices.First().SAHLReference)) ||
                    m.Contains(string.Format("Failed to add invoice {0} to payment batch", failingInvoices.Last().SAHLReference))
                  )
                , null
             );
        };

        private It should_return_failing_invoices_workflow_cases_to_payment_approved = () =>
        {
            x2WorkflowManager.Received(failingInvoices.Count()).ReversePayment(
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