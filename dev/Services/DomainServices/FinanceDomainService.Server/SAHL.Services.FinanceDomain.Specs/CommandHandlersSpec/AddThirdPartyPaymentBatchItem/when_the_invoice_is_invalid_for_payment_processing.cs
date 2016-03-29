using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AddThirdPartyPaymentBatchItem
{
    public class when_the_invoice_is_invalid_for_payment_processing : WithFakes
    {
        private static AddThirdPartyInvoiceToPaymentBatchCommand command;
        private static AddThirdPartyInvoiceToPaymentBatchCommandHandler handler;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;

        private static IEventRaiser eventRaiser;
        private static ICATSServiceClient catsServiceClient;
        private static IThirdPartyInvoiceDataManager dataManager;

        private static int thirdpartyInvoiceKey;
        private static int catsPaymentBatchKey;
        private static AddCATSPaymentBatchItemCommand catsServiceCommand;

        private static CATSPaymentBatchItemModel catsPaymentBatchItemModel;
        private static ThirdPartyInvoicePaymentBatchItem catsPaymentBatchItemDataModel;

        private Establish context = () =>
        {
            catsServiceClient = An<ICATSServiceClient>();
            eventRaiser = An<IEventRaiser>();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            thirdpartyInvoiceKey = 1432;
            catsPaymentBatchKey = 23;

            command = new AddThirdPartyInvoiceToPaymentBatchCommand(catsPaymentBatchKey, thirdpartyInvoiceKey);
            handler = new AddThirdPartyInvoiceToPaymentBatchCommandHandler(dataManager, catsServiceClient, eventRaiser);

            metadata = An<IServiceRequestMetadata>();
            messages = new SystemMessageCollection();

            catsPaymentBatchItemModel = new CATSPaymentBatchItemModel(
                1,
                1,
                1,
                1,
                1.0m,
                1,
                1,
                1,
                "",
                "",
                "Straus Daly",
                "",
                "Payment Ref: Paint Job",
                true
                );

            catsServiceCommand = new AddCATSPaymentBatchItemCommand(catsPaymentBatchItemModel);

            catsPaymentBatchItemDataModel = null;

            dataManager.WhenToldTo(y => y.GetCatsPaymentBatchItemInformation(Param.IsAny<int>(), Param.IsAny<int>())).Return(catsPaymentBatchItemDataModel);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_prepare_sufficient_invoice_information_to_include_in_payment_batch = () =>
        {
            dataManager.WasToldTo(y => y.GetCatsPaymentBatchItemInformation(
                  Param<int>.Matches(m => m == catsPaymentBatchKey),
                  Param<int>.Matches(m => m == thirdpartyInvoiceKey)
             ));
        };

        private It should_not_call_cats_service_with_command = () =>
        {
            catsServiceClient.WasNotToldTo(x => x.PerformCommand(Param.IsAny<AddCATSPaymentBatchItemCommand>(), metadata));
        };

        private It should_return_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(string.Format("The Invoice: {0}, is not valid for payment processing.", command.ThirdPartyInvoiceKey));
        };
    }
}