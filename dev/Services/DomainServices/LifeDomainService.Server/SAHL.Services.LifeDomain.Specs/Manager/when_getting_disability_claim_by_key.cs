using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Managers.Statements;

namespace SAHL.Services.LifeDomain.Specs.ManagerSpecs
{
    public class when_getting_disability_claim_by_key : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static FakeDbFactory dbFactory;
        private static DisabilityClaimDetailModel disabilityClaimModel;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            lifeDomainDataManager = new LifeDomainDataManager(dbFactory);
        };

        private Because of = () =>
        {
            disabilityClaimModel = lifeDomainDataManager.GetDisabilityClaimDetailByKey(Param<int>.IsAnything);
        };

        private It should_use_the_sql_statement = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetDisabilityClaimDetailByKeyStatement>.IsAnything));
        };
    }
}