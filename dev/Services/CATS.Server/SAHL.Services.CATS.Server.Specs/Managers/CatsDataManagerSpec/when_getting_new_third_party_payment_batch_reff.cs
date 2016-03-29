using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_getting_new_third_party_payment_batch_reff : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static IDbFactory dbFactory;
        private static int batchKey, expectedBatchKey;

        Establish context = () =>
        {
            expectedBatchKey = 108;
            dbFactory = new FakeDbFactory();
            dataManager = new CATSDataManager(dbFactory);
            dbFactory.NewDb().InAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<GetNewThirdPartyPaymentReferenceStatement>())).Return(expectedBatchKey);
        };

        Because of = () =>
        {
            batchKey = dataManager.GetNewThirdPartyPaymentBatchReference(CATSPaymentBatchType.ThirdPartyInvoice);
        };

        It should_return_the_batch_key_as_the_reference = () =>
        {
            batchKey.ShouldEqual(expectedBatchKey);
        };

        It should_should_get_the_batch_from_the_db = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.SelectOne<int>(Param.IsAny<GetNewThirdPartyPaymentReferenceStatement>()));
        };

        It should_comple_the_transaction = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
        };

    }
}
