using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class RemoveThirdPartyInvoiceFromPaymentBatchCommandHandler :
        IDomainServiceCommandHandler<RemoveThirdPartyInvoiceFromPaymentBatchCommand, ThirdPartyInvoiceRemovedFromBatchEvent>
    {
        private ICATSServiceClient catsServiceClient;
        private IEventRaiser eventRaiser;
        private IThirdPartyInvoiceDataManager dataManager;

        public RemoveThirdPartyInvoiceFromPaymentBatchCommandHandler(
              ICATSServiceClient catsServiceClient
            , IThirdPartyInvoiceDataManager dataManager
            , IEventRaiser eventRaiser
         )
        {
            this.catsServiceClient = catsServiceClient;
            this.eventRaiser = eventRaiser;
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveThirdPartyInvoiceFromPaymentBatchCommand command,
            IServiceRequestMetadata metadata)
        {
            ThirdPartyInvoicePaymentBatchItem catsPaymentBatchItem = dataManager.GetCatsPaymentBatchItemInformation(command.CATSPaymentBatchKey, command.ThirdPartyInvoiceKey);
            var messages = catsServiceClient.PerformCommand(new RemoveCATSPaymentBatchItemCommand(command.CATSPaymentBatchKey,
                command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice), metadata);

            if (!messages.HasErrors)
            {
                var eventOccuranceDate = DateTime.Now;
                eventRaiser.RaiseEvent(eventOccuranceDate, new ThirdPartyInvoiceRemovedFromBatchEvent(
                    eventOccuranceDate,
                    command.CATSPaymentBatchKey,
                    command.ThirdPartyInvoiceKey,
                    catsPaymentBatchItem.InvoiceTotal),
                    command.ThirdPartyInvoiceKey,
                    (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }
            return messages;
        }
    }
}