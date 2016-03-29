using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec.AddThirdPartyInvoiceBatchPayment
{
    public class when_adding_third_party_invoice_batch_payment_item : WithFakes
    {
        private static ICATSServiceClient _catsService;
        private static IServiceRequestMetadata _serviceRequestMetadata;
        private static ISystemMessageCollection _messages;

        private static AddCATSPaymentBatchItemCommand _command;
        private static AddCATSPaymentBatchItemCommandHandler _handler;
        private static CATSPaymentBatchItemModel _thirdPartyInvoicePaymentBatchItemModel;

        private static ICATSDataManager _catsDataManager;
        private static int _legalEntityKey;

        private Establish context = () =>
        {
            _serviceRequestMetadata = An<IServiceRequestMetadata>();
            _catsService = An<ICATSServiceClient>();
            _messages = SystemMessageCollection.Empty();
            _catsDataManager = An<ICATSDataManager>();

            _legalEntityKey = 2004;

            _thirdPartyInvoicePaymentBatchItemModel = new CATSPaymentBatchItemModel(_legalEntityKey, 1, 1, 1, 1, 49, 1, 1, "reference", "SAHL      SPV 1",
                "Straus Daly", "Invoice Num", "straus@sd.co.za", true);
            _command = new AddCATSPaymentBatchItemCommand(_thirdPartyInvoicePaymentBatchItemModel);
            _handler = new AddCATSPaymentBatchItemCommandHandler(_catsDataManager);

            _catsDataManager.WhenToldTo(x => x.InsertThirdPartyInvoicePaymentBatchItem(_thirdPartyInvoicePaymentBatchItemModel));
        };

        private Because of = () =>
        {
            _messages = _handler.HandleCommand(_command, _serviceRequestMetadata);
        };

        private It should_save_a_third_party_invoice_payment_batch_item = () =>
        {
            _catsDataManager.WasToldTo(
                x => x.InsertThirdPartyInvoicePaymentBatchItem(_thirdPartyInvoicePaymentBatchItemModel));
        };

        private It should_not_return_any_error_messages = () => _messages.HasErrors.ShouldBeFalse();


    }
}