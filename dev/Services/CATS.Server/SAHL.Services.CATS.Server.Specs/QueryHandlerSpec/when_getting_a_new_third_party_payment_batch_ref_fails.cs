using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.QueryHandlers;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.QueryHandlerSpec
{
    public class when_getting_a_new_third_party_payment_batch_ref_fails : WithFakes
    {
        private static ICatsDataManager catsDataManager;
        private static GetNewThirdPartyPaymentBatchReferenceQueryHandler handler;
        private static GetNewThirdPartyPaymentBatchReferenceQuery query;
        private static GetNewThirdPartyPaymentBatchReferenceQueryResult result;
        private static int batchReferenceNumber;
        private static ISystemMessageCollection messages;

        Establish context = () =>
        {
            query = new GetNewThirdPartyPaymentBatchReferenceQuery();
            catsDataManager = An<ICatsDataManager>();
            handler = new GetNewThirdPartyPaymentBatchReferenceQueryHandler(catsDataManager);

            catsDataManager.WhenToldTo(x => x.GetNewThirdPartyPaymentBatchReference()).Return(0);

        };

        Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        It should_get_the_new_paymet_batch_refference = () =>
        {
            catsDataManager.WasToldTo(x => x.GetNewThirdPartyPaymentBatchReference());
        };

        It should_return_the_batch_reference_number = () =>
        {
            query.Result.Results.First().BatchKey.ShouldEqual(0);
        };

        It should_contain_an_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.AllMessages.Any(y => y.Message == "Failed to retrieve the batch key").ShouldBeTrue();
        };
    }
}
