using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
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
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_handling_payment_batch_processed_event_archiving_fails : PayThirdPartyInvoiceProcessSpec
    {
        private static CATSPaymentBatchProcessedEvent paymentBatchProcessedEvent;
        private static long case1Instance, case2Instance;
        private static int batchReference;
        private static CATSPaymentBatchTypeDataModel thirdPartyCATSPaymentBatch;
        public static List<PayThirdPartyInvoiceModel> invoices;
        private static PayThirdPartyInvoiceProcessModel dataModel;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;
        private static ISystemMessageCollection errorMessages;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            thirdPartyCATSPaymentBatch = new CATSPaymentBatchTypeDataModel("ThirdpartyInvoice", "SHL04", "SSVS", (int)CATsEnvironment.Live, 1);

            case1Instance = 1000000000000000001;
            case2Instance = 1000000000000000101;
            batchReference = 15;

            payThirdPartyInvoiceStateMachine.BatchReference.Returns(batchReference);

            invoices = new List<PayThirdPartyInvoiceModel>
                            {
                                   new PayThirdPartyInvoiceModel(18377, case1Instance, 1399640, "SAHL-2015/07/359", PaymentProcessStep.ReadyForArchiving)
                                ,  new PayThirdPartyInvoiceModel(18378, case2Instance, 4077922, "SAHL-2015/07/335", PaymentProcessStep.ReadyForArchiving )
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

            errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Error occured", SystemMessageSeverityEnum.Error));
            x2WorkflowManager.WhenToldTo(x => x.ArchiveThirdPartyInvoice(Param.IsAny<long>(), Param.IsAny<int>(), Param.IsAny<int>(),
                 Param.IsAny<DomainProcessServiceRequestMetadata>())).Return(errorMessages);

            var systemMessageQueue = Substitute.For<ConcurrentQueue<ISystemMessageCollection>>();
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Returns(systemMessageQueue);

            ConcurrentQueue<int> stuckInvoiceQueue = new ConcurrentQueue<int>();
            Parallel.ForEach(dataModel.InvoiceCollection, (invoice) =>
            {
                invoice.StepInProcess = PaymentProcessStep.ReadyForArchiving;
                stuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
            });
            payThirdPartyInvoiceStateMachine.WhenToldTo(y => y.StuckInvoiceQueue).Return(stuckInvoiceQueue);
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(paymentBatchProcessedEvent, metadata);
        };


        private It should_archive_all_workflow_cases_for_invoices_in_batch = () =>
        {
            x2WorkflowManager.WasToldTo(x => x.ArchiveThirdPartyInvoice(Param.IsAny<long>(), Param.IsAny<int>(), Param.IsAny<int>(),
                 Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
        private It should_queue_the_messages = () =>
        {
            payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Received().Enqueue(errorMessages);
        };
        
    }
}