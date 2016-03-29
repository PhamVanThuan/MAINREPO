using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.LifeDomain.Managers;
using System;
using System.Linq;

namespace SAHL.Services.LifeDomain.Specs.Manager
{
    public class when_deleting_a_disability_claim : WithFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static FakeDbFactory dbFactory;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            disabilityClaimKey = 6566451;
            dbFactory = new FakeDbFactory();
            lifeDomainDataManager = new LifeDomainDataManager(dbFactory);
        };

        private Because of = () =>
        {
            lifeDomainDataManager.DeleteDisabilityClaim(disabilityClaimKey);
        };

        private It remove_the_record_provided_by_key = () =>
        {
            dbFactory.FakedDb.InAppContext().Received().DeleteByKey<DisabilityClaimDataModel, int>(disabilityClaimKey);
        };
    }
}