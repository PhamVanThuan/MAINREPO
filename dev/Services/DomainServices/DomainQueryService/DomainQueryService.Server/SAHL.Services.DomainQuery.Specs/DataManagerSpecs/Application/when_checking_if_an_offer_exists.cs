using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DomainQuery.Managers.Application;
using SAHL.Services.DomainQuery.Managers.Application.Statements;

namespace SAHL.Services.DomainQuery.Specs.DataManagerSpecs.Application
{
    public class when_checking_if_an_offer_exists : WithFakes
    {
        private static ApplicationDataManager dataManager;
        private static int offerKey;
        private static bool result;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new ApplicationDataManager(fakeDbFactory);
            offerKey = 35;
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param<DoesOfferExistForOfferKeyStatement>.Matches(y => y.OfferKey == offerKey))).Return(1);
        };

        private Because of = () =>
        {
            result = dataManager.DoesOfferExist(offerKey);
        };

        private It should_select_from_offer_against_the_key = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<int>(Param<DoesOfferExistForOfferKeyStatement>.Matches(y => y.OfferKey == offerKey)));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}