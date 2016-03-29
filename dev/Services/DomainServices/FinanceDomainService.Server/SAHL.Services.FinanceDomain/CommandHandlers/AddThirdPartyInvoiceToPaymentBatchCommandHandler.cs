using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class AddThirdPartyInvoiceToPaymentBatchCommandHandler :
        IDomainServiceCommandHandler<AddThirdPartyInvoiceToPaymentBatchCommand, ThirdPartyInvoiceAddedToBatchEvent>
    {
        private readonly IEventRaiser eventRaiser;
        private ICATSServiceClient catsServiceClient;
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public AddThirdPartyInvoiceToPaymentBatchCommandHandler(
              IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager
            , ICATSServiceClient catsServiceClient
            , IEventRaiser eventRaiser
         )
        {
            this.catsServiceClient = catsServiceClient;
            this.eventRaiser = eventRaiser;
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public ISystemMessageCollection HandleCommand(AddThirdPartyInvoiceToPaymentBatchCommand command,
            IServiceRequestMetadata metadata)
        {
            ThirdPartyInvoicePaymentBatchItem thirdPartyInvoicesBatchItems = thirdPartyInvoiceDataManager.GetCatsPaymentBatchItemInformation(command.CATSPaymentBatchKey, command.ThirdPartyInvoiceKey);


            ISystemMessageCollection messages = new SystemMessageCollection();
            if (thirdPartyInvoicesBatchItems == null)
            {
                messages.AddMessage(new SystemMessage(string.Format("The Invoice: {0}, is not valid for payment processing.", command.ThirdPartyInvoiceKey)
                    , SystemMessageSeverityEnum.Error));
                return messages;
            }
            string paymentReference = "SA HOME LOANS";
            var catsServiceCommand = new AddCATSPaymentBatchItemCommand(new CATSPaymentBatchItemModel(
                thirdPartyInvoicesBatchItems.LegalEntityKey,
                thirdPartyInvoicesBatchItems.GenericKey,
                thirdPartyInvoicesBatchItems.GenericTypeKey,
                thirdPartyInvoicesBatchItems.AccountKey,
                thirdPartyInvoicesBatchItems.InvoiceTotal,
                thirdPartyInvoicesBatchItems.ThirdPartyPaymentBatchKey,
                thirdPartyInvoicesBatchItems.SourceBankAccountKey,
                thirdPartyInvoicesBatchItems.TargetBankAccountKey,
                thirdPartyInvoicesBatchItems.SahlReferenceNumber,
                thirdPartyInvoicesBatchItems.SourceReference,
                thirdPartyInvoicesBatchItems.TargetName,
                paymentReference,
                thirdPartyInvoicesBatchItems.EmailAddress, true
                ));

            messages = catsServiceClient.PerformCommand(catsServiceCommand, metadata);

            if (!messages.HasErrors)
            {
                var eventOccuranceDate = DateTime.Now;
                eventRaiser.RaiseEvent(eventOccuranceDate, new ThirdPartyInvoiceAddedToBatchEvent(eventOccuranceDate,
                    command.CATSPaymentBatchKey, command.ThirdPartyInvoiceKey, thirdPartyInvoicesBatchItems.InvoiceTotal),
                    command.ThirdPartyInvoiceKey,
                    (int)GenericKeyType.ThirdPartyInvoice, metadata);
            }

            return messages;
        }
    }
}