using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcesses.Rules;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public class PayAttorneyInvoiceProcess : PayThirdPartyInvoiceDomainProcess<PayThirdPartyInvoiceProcessModel>
    {

        public IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;
        public PayAttorneyInvoiceProcess(
              IPayThirdPartyInvoiceStateMachine payThirdPartyInvoiceStateMachine
            , IFinanceDomainServiceClient financeDomainService
            , ICombGuid combGuidGenerator
            , ICATSServiceClient catsService
            , IX2WorkflowManager x2WorkflowManager
            , ICommunicationManager communicationManager
            , IRawLogger rawLogger
            , ILoggerSource loggerSource
            , ILoggerAppSource loggerAppSource
            , IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager)
            : base(payThirdPartyInvoiceStateMachine, financeDomainService, combGuidGenerator, catsService, x2WorkflowManager, communicationManager, rawLogger, loggerSource, loggerAppSource)
        {
            this.domainRuleManager = domainRuleManager;
            domainRuleManager.RegisterRule(new NoCatsFileForProfileShouldBePendindingRule(catsService));
            domainRuleManager.RegisterRule(new PreviousBatchFileShouldNotHaveFailedRule(catsService));
            domainRuleManager.RegisterRule(new PayAttorneyInvoiceProcessShouldRunOnceADayRule(catsService));
        }

        public override void OnInternalStart()
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(systemMessages, this.DataModel);
            if (!systemMessages.HasErrors)
            {
                var queryCorrelationId = combGuidGenerator.Generate();
                var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, queryCorrelationId);

                var query = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);

                systemMessages = catsService.PerformQuery(query);

                if (systemMessages.HasErrors || query.Result == null || !query.Result.Results.Any())
                {
                    var exception = systemMessages.HasErrors ? new Exception("Failed to acquire a batch reference"
                                                                  , new Exception(systemMessages.ErrorMessages().Aggregate<ISystemMessage, String>(string.Empty, (f, s) =>
                                                                  {
                                                                      return string.Format("{0},{1}", f, s.Message);
                                                                  })))
                                                             : new Exception("Failed to acquire a batch reference");

                    systemMessages.AddMessage(new SystemMessage(exception.ToString(), SystemMessageSeverityEnum.Error));
                    HandleErrors(systemMessages);
                }

                var batchReference = query.Result.Results.First().BatchKey;
                payThirdPartyInvoiceStateMachine.FireStateMachineTriggerWithKey(payThirdPartyInvoiceStateMachine.BatchReferenceSetTrigger, queryCorrelationId, batchReference);

                Parallel.ForEach(this.DataModel.InvoiceCollection, (invoice) =>
                {
                    var commandCorrelationId = combGuidGenerator.Generate();
                    serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, commandCorrelationId);
                    var messageCollection = SystemMessageCollection.Empty();
                    messageCollection.Aggregate(x2WorkflowManager.ProcessThirdPartyInvoicePayment(
                          invoice.InstanceId
                        , invoice.AccountNumber
                        , invoice.ThirdPartyInvoiceKey
                        , serviceRequestMetadata
                    ));

                    invoice.StepInProcess = PaymentProcessStep.ReadyForBatching;
                    if (messageCollection.HasErrors)
                    {
                        payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Enqueue(messageCollection);
                        payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
                        invoice.StepInProcess = PaymentProcessStep.PreparingWorkflowCaseFailed;
                    }
                });

                bool allInvoicesStuck = payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Count == this.DataModel.InvoiceCollection.Count;
                if (!allInvoicesStuck)
                {
                    payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(
                        payThirdPartyInvoiceStateMachine.BatchReadyForProcessingTrigger
                        , combGuidGenerator.Generate());
                }

                Parallel.ForEach(this.DataModel.InvoiceCollection.Where(y => y.StepInProcess == PaymentProcessStep.ReadyForBatching), (invoice) =>
                {
                    var commandCorrelationId = combGuidGenerator.Generate();
                    serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, commandCorrelationId);
                    var messageCollection = SystemMessageCollection.Empty();
                    var addThirdPartyInvoiceToBatchCommand = new AddThirdPartyInvoiceToPaymentBatchCommand(payThirdPartyInvoiceStateMachine.BatchReference, invoice.ThirdPartyInvoiceKey);

                    payThirdPartyInvoiceStateMachine.RecordRequestSent(addThirdPartyInvoiceToBatchCommand.GetType(), commandCorrelationId);
                    messageCollection.Aggregate(financeDomainService.PerformCommand(addThirdPartyInvoiceToBatchCommand, serviceRequestMetadata));

                    invoice.StepInProcess = PaymentProcessStep.ReadyForPostingTransation;

                    if (messageCollection.HasErrors)
                    {
                        payThirdPartyInvoiceStateMachine.RecordErrorResponseOrCommandFailed(commandCorrelationId);
                        payThirdPartyInvoiceStateMachine.SystemMessagesQueue.Enqueue(messageCollection);
                        payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Enqueue(invoice.ThirdPartyInvoiceKey);
                        invoice.StepInProcess = PaymentProcessStep.BatchingFailed;
                    }
                });

                if (payThirdPartyInvoiceStateMachine.StuckInvoiceQueue.Count == this.DataModel.InvoiceCollection.Count)
                {
                    CompleteDomainProcess();
                }
            }
            else
            {
                HandleErrors(systemMessages);
            }
        }

        private void HandleErrors(ISystemMessageCollection systemMessages)
        {
            var messages = new StringBuilder();
            foreach (var msg in systemMessages.AllMessages)
            {
                messages.Append(msg.Message);
            }

            var exception = new Exception(messages.ToString());

            payThirdPartyInvoiceStateMachine.FireStateMachineTrigger(
                    payThirdPartyInvoiceStateMachine.CriticalErrorReportedTrigger
                    , this.DomainProcessId);

            OnErrorOccurred(this.DomainProcessId, exception.ToString());
            throw exception;
        }

    }
}