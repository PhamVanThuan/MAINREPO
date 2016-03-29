using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.LifeDataManager;
using SAHL.DomainServiceChecks.Managers.LifeDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.Life
{
    public class when_getting_a_disability_claim : WithFakes
    {
        private static LifeDataManager lifeDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            lifeDataManager = new LifeDataManager(fakeDbFactory);
            disabilityClaimKey = 1;
        };

        private Because of = () =>
        {
            lifeDataManager.GetDisabilityClaimByKey(disabilityClaimKey);
        };

        private It should_check_for_a_disability_claim = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<GetDisabilityClaimByKeyStatement>(y => y.DisabilityClaimKey == disabilityClaimKey)));
        };
    }
}