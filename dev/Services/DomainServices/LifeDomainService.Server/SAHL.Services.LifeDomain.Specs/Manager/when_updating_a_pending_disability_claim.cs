using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Managers.Statements;
using System;

namespace SAHL.Services.LifeDomain.Specs.ManagerSpecs
{
    public class when_updating_a_pending_disability_claim : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDoaminDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            lifeDoaminDataManager = new LifeDomainDataManager(fakeDbFactory);
            disabilityClaimKey = 1;
        };

        private Because of = () =>
        {
            lifeDoaminDataManager.UpdatePendingDisabilityClaim(disabilityClaimKey, Param.IsAny<DateTime>(), Param.IsAny<int>(), Param.IsAny<string>(),
                Param.IsAny<string>(), Param.IsAny<DateTime>(), Param.IsAny<DateTime>());
        };

        private It should_execute_the_update_pending_disability_claim_statement = () =>
        {
            fakeDbFactory.FakedDb.DbContext.WasToldTo(x => x.ExecuteNonQuery(Arg.Is<UpdatePendingDisabilityClaimStatement>(y => y.DisabilityClaimKey == disabilityClaimKey)));
        };
    }
}