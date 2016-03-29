using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.RemoveThirdPartyPaymentBatchItem
{
    public class when_removing_a_third_party_payment_batch_item : WithFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ICATSServiceClient catsServiceClient;
        private static IEventRaiser eventRaiser;
        private static int thirdPartyInvoiceKey;
        private static int catsPaymentBatchKey;
        private static RemoveThirdPartyInvoiceFromPaymentBatchCommand command;
        private static RemoveThirdPartyInvoiceFromPaymentBatchCommandHandler handler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static ThirdPartyInvoicePaymentBatchItem thirdPartyInvoicePaymentBatchItemModel;

        private Establish context = () =>
        {
            dataManager = An<IThirdPartyInvoiceDataManager>();
            catsServiceClient = An<ICATSServiceClient>();
            eventRaiser = An<IEventRaiser>();
            thirdPartyInvoiceKey = 7438;
            catsPaymentBatchKey = 756;

            command = new RemoveThirdPartyInvoiceFromPaymentBatchCommand(catsPaymentBatchKey, thirdPartyInvoiceKey);
            handler = new RemoveThirdPartyInvoiceFromPaymentBatchCommandHandler(catsServiceClient, dataManager,
                eventRaiser);

            metadata = An<IServiceRequestMetadata>();
            messages = new SystemMessageCollection();

            thirdPartyInvoicePaymentBatchItemModel = new ThirdPartyInvoicePaymentBatchItem(1, 1, 1, 1, 1.0m, 1, 1, "", "", 1, "", "", "", "Payment Reference");

            dataManager.WhenToldTo(y => y.GetCatsPaymentBatchItemInformation(Param.IsAny<int>(), Param.IsAny<int>())).Return(thirdPartyInvoicePaymentBatchItemModel);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_collect_invoice_details_for_event = () =>
        {
            dataManager.WasToldTo(y => y.GetCatsPaymentBatchItemInformation(
                  Param<int>.Matches(m => m == catsPaymentBatchKey)
                , Param<int>.Matches(m => m == thirdPartyInvoiceKey)
             ));
        };

        private It should_call_cats_service_with_command = () =>
        {
            catsServiceClient.WasToldTo(
                x =>
                    x.PerformCommand(
                        Param<RemoveCATSPaymentBatchItemCommand>.Matches(y =>
                            y.GenericTypeKey == (int)GenericKeyType.ThirdPartyInvoice &&
                            y.GenericKey == thirdPartyInvoiceKey
                            ),
                         metadata));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}