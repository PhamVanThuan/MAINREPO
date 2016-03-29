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
    public class when_updating_a_disability_claim_status : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static FakeDbFactory dbFactory;
        private static int disabilityClaimKey;
       
        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            lifeDomainDataManager = new LifeDomainDataManager(dbFactory);
            disabilityClaimKey = 1;
        };

        private Because of = () =>
        {
            lifeDomainDataManager.UpdateDisabilityClaimStatus(disabilityClaimKey, Param.IsAny<DisabilityClaimStatus>());
        };

        private It should_execute_the_update_disability_claim_status_statement = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.ExecuteNonQuery(Arg.Is<UpdateDisabilityClaimStatusStatement>(y => y.DisabilityClaimKey == disabilityClaimKey)));
        };
    }
}