using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Managers.Statements;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.Specs.ManagerSpecs
{
    public class when_getting_disability_claim_history : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<DisabilityClaimDetailModel> disabilityClaims;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            lifeDomainDataManager = new LifeDomainDataManager(dbFactory);
        };

        private Because of = () =>
        {
            disabilityClaims = lifeDomainDataManager.GetDisabilityClaimHistory(Param<int>.IsAnything);
        };

        private It should_use_the_sql_statement = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select(Param<GetDisabilityClaimHistoryStatement>.IsAnything));
        };
    }
}