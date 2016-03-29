using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.QueryHandlers;
using SAHL.Services.Interfaces.CATS.Queries;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.QueryHandlerSpec
{
    public class when_getting_a_new_third_party_payment_batch_ref : WithFakes
    {
        private static ICATSDataManager catsDataManager;
        private static GetNewThirdPartyPaymentBatchReferenceQueryHandler handler;
        private static GetNewThirdPartyPaymentBatchReferenceQuery query;
        private static int batchReferenceNumber;
        private static ISystemMessageCollection messages;

        Establish context = () =>
        {
            query = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            catsDataManager = An<ICATSDataManager>();
            handler = new GetNewThirdPartyPaymentBatchReferenceQueryHandler(catsDataManager);
            batchReferenceNumber = 108;

            catsDataManager.WhenToldTo(x => x.GetNewThirdPartyPaymentBatchReference(CATSPaymentBatchType.ThirdPartyInvoice)).Return(batchReferenceNumber);

        };

        Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        It should_get_the_new_paymet_batch_refference = () =>
        {
            catsDataManager.WasToldTo(x => x.GetNewThirdPartyPaymentBatchReference(CATSPaymentBatchType.ThirdPartyInvoice));
        };

        It should_return_the_batch_reference_number = () =>
        {
            query.Result.Results.First().BatchKey.ShouldEqual(batchReferenceNumber);
        };

        It should_not_contain_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
