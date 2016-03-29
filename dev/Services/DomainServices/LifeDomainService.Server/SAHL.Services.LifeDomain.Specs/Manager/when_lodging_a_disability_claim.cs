using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Managers.Statements;
using System;

namespace SAHL.Services.LifeDomain.Specs.ManagerSpecs
{
    public class when_lodging_a_disability_claim : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static FakeDbFactory dbFactory;
        private static int lifeAccountKey;
        private static int claimantLegalEntityKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            lifeDomainDataManager = new LifeDomainDataManager(dbFactory);

            lifeAccountKey = 1;
            claimantLegalEntityKey = 99;
        };

        private Because of = () =>
        {
            lifeDomainDataManager.LodgeDisabilityClaim(lifeAccountKey, claimantLegalEntityKey);
        };
        private It should_execute_the_lodge_disability_claim_statement = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.SelectOne<int>(Arg.Is<LodgeDisabilityClaimStatement>(y => y.AccountKey == lifeAccountKey && y.LegalEntityKey == claimantLegalEntityKey)));
        };
    }
}