using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec.RemoveCatsPaymentBatchItem
{
    class when_removing_cats_payment_batch_item : WithFakes
    {
        private static ICATSServiceClient _catsService;
        private static IServiceRequestMetadata _serviceRequestMetadata;
        private static ISystemMessageCollection _messages;

        private static RemoveCATSPaymentBatchItemCommand _command;
        private static RemoveCATSPaymentBatchItemCommandHandler _handler;

        private static ICATSDataManager _catsDataManager;
        private static int _thirdPartyInvoiceKey;
        private static int _thirdPartyBatchKey;
        private static int _genericTypeKey = (int)GenericKeyType.ThirdPartyInvoice;

        private Establish context = () =>
        {
            _serviceRequestMetadata = An<IServiceRequestMetadata>();
            _catsService = An<ICATSServiceClient>();
            _messages = SystemMessageCollection.Empty();
            _catsDataManager = An<ICATSDataManager>();

            _thirdPartyInvoiceKey = 1;
            _thirdPartyBatchKey = 1;

            _command = new RemoveCATSPaymentBatchItemCommand(_thirdPartyBatchKey,_thirdPartyInvoiceKey, _genericTypeKey);
            _handler = new RemoveCATSPaymentBatchItemCommandHandler(_catsDataManager);

            _catsDataManager.WhenToldTo(x => x.RemoveCATSPaymentBatchItem(_thirdPartyBatchKey, _thirdPartyInvoiceKey, _genericTypeKey));
        };

        private Because of = () =>
        {
            _messages = _handler.HandleCommand(_command, _serviceRequestMetadata);
        };

        private It should_remove_a_cats_payment_batch_item = () =>
        {
            _catsDataManager.WasToldTo(
                x => x.RemoveCATSPaymentBatchItem(_thirdPartyBatchKey, _thirdPartyInvoiceKey, _genericTypeKey));
        };

        private It should_not_return_any_error_messages = () => _messages.HasErrors.ShouldBeFalse();
        
    }
    
}
