using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class SummarisePaymentsToThirdPartyCommandHandler : IServiceCommandHandler<SummarisePaymentsToThirdPartyCommand>
    {
        private IThirdPartyInvoiceManager thirdPartyInvoiceManager;
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private ICATSServiceClient catsServiceClient;
        private IEventRaiser eventRaiser;

        public SummarisePaymentsToThirdPartyCommandHandler(IThirdPartyInvoiceManager thirdPartyInvoiceManager, IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager,
            ICATSServiceClient catsServiceClient, IEventRaiser eventRaiser)
        {
            this.thirdPartyInvoiceManager = thirdPartyInvoiceManager;
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            this.catsServiceClient = catsServiceClient;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(SummarisePaymentsToThirdPartyCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var getCatsPaymentBatchItemsByBatchReferenceQuery = new GetCatsPaymentBatchItemsByBatchReferenceQuery(command.CATSPaymentBatchKey);
            messages.Aggregate(
                catsServiceClient.PerformQuery(getCatsPaymentBatchItemsByBatchReferenceQuery));
            if (messages.HasErrors)
            {
                return messages;
            }

            if (getCatsPaymentBatchItemsByBatchReferenceQuery.Result == null || getCatsPaymentBatchItemsByBatchReferenceQuery.Result.Results.Count() == 0)
            {
                messages.AddMessage(
                    new SystemMessage(string.Format("No batch line items could be found for the batch {0}.", command.CATSPaymentBatchKey), SystemMessageSeverityEnum.Error));
                return messages;
            }
            var batchPaymentItems = getCatsPaymentBatchItemsByBatchReferenceQuery.Result.Results;

            var thirdPartyInvoiceKeys = batchPaymentItems.Where(y => y.GenericTypeKey == (int)GenericKeyType.ThirdPartyInvoice).Select(g => g.GenericKey).ToArray();
            var invoicePayments = thirdPartyInvoiceDataManager.GetThirdPartyInvoicePaymentInformation(thirdPartyInvoiceKeys);

            var groupedThirdPartyPayments = thirdPartyInvoiceManager.GroupThirdPartyPaymentInvoicesByThirdParty(invoicePayments);

            var systemMessages = new ConcurrentQueue<string>();
            Parallel.ForEach(groupedThirdPartyPayments, paymentGroup =>
            {
                var @event = new SummarisedPaymentsToThirdPartyEvent(DateTime.Now, paymentGroup.Value.First().EmailAddress,
                   paymentGroup.Value);

                try
                {
                    eventRaiser.RaiseEvent(DateTime.Now, @event, paymentGroup.Value.First().ThirdPartyKey, (int)GenericKeyType.ThirdParty, metadata);
                }
                catch (Exception ex)
                {
                    // Maybe log ex since we're swallowing it
                    systemMessages.Enqueue(string.Format("Attorney Name: {0}\n {1}", paymentGroup.Value.First().FirmName
                        , paymentGroup.Value.Select(y => string.Format("{0}\t{1}\t{2}\t{3}", y.AccountNumber, y.InvoiceNumber, y.InvoiceAmountIncludingVat)
                     ).Aggregate((i, j) => i + "\n" + j)));
                }
            });

            messages.AddMessages(systemMessages.Select(y => new SystemMessage(y, SystemMessageSeverityEnum.Error)));
            return messages;
        }
    }
}