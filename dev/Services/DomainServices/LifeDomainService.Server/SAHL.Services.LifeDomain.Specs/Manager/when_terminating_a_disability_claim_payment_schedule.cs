using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Managers.Statements;
using SAHL.Services.LifeDomain.QueryStatements;
using System;

namespace SAHL.Services.LifeDomain.Specs.ManagerSpecs
{
    public class when_terminating_a_disability_claim_payment_schedule : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static FakeDbFactory dbFactory;
        private static int disabilityClaimKey;
        private static string adUserName;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            lifeDomainDataManager = new LifeDomainDataManager(dbFactory);
            disabilityClaimKey = 1;
            adUserName = @"SAHL\CraigF";
        };

        private Because of = () =>
        {
            lifeDomainDataManager.TerminateDisabilityClaimPaymentSchedule(disabilityClaimKey, adUserName);
        };

        private It should_query_to_terminate_the_payment_schedule = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<string>(Arg.Is<TerminateDisabilityClaimPaymentScheduleStatement>(y => y.DisabilityClaimKey == disabilityClaimKey)));
        };
    }
}