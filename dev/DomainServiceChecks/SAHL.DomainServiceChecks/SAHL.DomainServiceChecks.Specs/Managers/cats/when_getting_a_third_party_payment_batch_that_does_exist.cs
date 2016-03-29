using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.CatsDataManager;
using SAHL.DomainServiceChecks.Managers.CatsDataManager.Statements;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.cats
{
    public class when_getting_a_third_party_payment_batch_that_does_exist : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static CatsDataManager dataManager;
        private static int ThirdPartyPaymentBatchKey;
        private static int CountThirdPartyPaymentBatch;
        private static bool ThirdPartyPaymentBatchExists;

        private Establish context = () =>
        {
            ThirdPartyPaymentBatchKey = 11;
            fakeDb = new FakeDbFactory();
            dataManager = new CatsDataManager(fakeDb);
            fakeDb.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.Select<int>(Arg.Any<DoesCATSPaymentBatchExistStatement>()))
                .Return(new[] { 1 });
        };

        private Because of = () =>
        {
            ThirdPartyPaymentBatchExists = dataManager.DoesCATSPaymentBatchExist(ThirdPartyPaymentBatchKey);
        };

        private It should_return_true = () =>
        {
            ThirdPartyPaymentBatchExists.ShouldBeTrue();
        };

        private It should_use_the_third_party_id_provided = () =>
        {
            fakeDb.FakedDb.InReadOnlyAppContext().Received().Select(Arg.Is<DoesCATSPaymentBatchExistStatement>(y => y.CATSPaymentBatchKey == ThirdPartyPaymentBatchKey));
        };
    }
}