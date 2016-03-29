using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Rules;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Events;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.CATS.Managers.CATS;
using System;
using System.Collections.Generic;

namespace SAHL.Services.CATS.CommandHandlers
{
    public class NotifyCATSPaymentBatchRecipientsCommandHandler : IServiceCommandHandler<NotifyCATSPaymentBatchRecipientsCommand>
    {
        private ICATSDataManager catsDataManager;
        private ICATSManager catsManager;
        private IEventRaiser eventRaiser;
        private IServiceCommandRouter serviceCommandRouter;
        private IServiceQueryRouter serviceQueryRouter;
        private IDomainRuleManager<CatsPaymentBatchRuleModel> domainRuleManager;

        public NotifyCATSPaymentBatchRecipientsCommandHandler(ICATSDataManager catsDataManager, ICATSManager catsManager, IServiceCommandRouter serviceCommandRouter,
            IServiceQueryRouter serviceQueryRouter, IEventRaiser eventRaiser, IDomainRuleManager<CatsPaymentBatchRuleModel> domainRuleManager)
        {
            this.catsDataManager = catsDataManager;
            this.catsManager = catsManager;
            this.serviceCommandRouter = serviceCommandRouter;
            this.eventRaiser = eventRaiser;
            this.serviceQueryRouter = serviceQueryRouter;
            this.domainRuleManager = domainRuleManager;

            domainRuleManager.RegisterRule(new BatchShouldBeInProcessedStateRule(catsDataManager));
        }

        public ISystemMessageCollection HandleCommand(NotifyCATSPaymentBatchRecipientsCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            domainRuleManager.ExecuteRules(messages, new CatsPaymentBatchRuleModel(command.CATSPaymentBatchKey));
            if (messages.HasErrors)
            {
                return messages;
            }

            var getCatsPaymentBatchItemsByBatchReferenceQuery = new GetCatsPaymentBatchItemsByBatchReferenceQuery(command.CATSPaymentBatchKey);
            messages.Aggregate(
                serviceQueryRouter.HandleQuery(getCatsPaymentBatchItemsByBatchReferenceQuery));

            if (messages.HasErrors)
            {
                return messages;
            }

            var batchPaymentItems = getCatsPaymentBatchItemsByBatchReferenceQuery.Result.Results;

            var groupedPayments = catsManager.GroupBatchPaymentsByRecipient(batchPaymentItems);
            var faileNotifications = new List<CATSPaymentBatchItemDataModel>();
            var successfulNotifications = new List<CATSPaymentBatchItemDataModel>();

            foreach (var payment in groupedPayments)
            {
                var summarisePaymentCommand = new SummarisePaymentsToRecipientCommand(payment.Key, payment.Value);
                var paymentSummaryMessages = serviceCommandRouter.HandleCommand(summarisePaymentCommand, metadata);
                if (paymentSummaryMessages.HasErrors)
                {
                    messages.AddMessages(paymentSummaryMessages.AllMessages);
                    faileNotifications.AddRange(payment.Value);
                }
                else
                {
                    successfulNotifications.AddRange(payment.Value);
                }
            }

            var @event = new CATSPaymentBatchRecipientsNotifiedEvent(DateTime.Now, successfulNotifications, faileNotifications);
            eventRaiser.RaiseEvent(DateTime.Now, @event, command.CATSPaymentBatchKey, (int)GenericKeyType.CATSPaymentBatch, metadata);

            return messages;
        }
    }
}