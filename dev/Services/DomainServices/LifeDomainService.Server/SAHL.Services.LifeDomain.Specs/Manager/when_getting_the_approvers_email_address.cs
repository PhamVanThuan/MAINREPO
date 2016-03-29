using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Managers.Statements;
using System;
using System.Linq;

namespace SAHL.Services.LifeDomain.Specs.Manager
{
    public class when_getting_the_approvers_email_address : WithFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static FakeDbFactory dbFactory;
        private static int disabilityClaimKey;
        private static string approverEmail;
        private static string result;

        private Establish context = () =>
        {
            approverEmail = "clintonspeed@gmail.com";
            disabilityClaimKey = 168754;
            dbFactory = new FakeDbFactory();
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param<GetEmailAddressForUserWhoApprovedDisabilityClaimStatement>.IsAnything))
                .Return(approverEmail);
            lifeDomainDataManager = new LifeDomainDataManager(dbFactory);
        };

        private Because of = () =>
        {
            result = lifeDomainDataManager.GetEmailAddressForUserWhoApprovedDisabilityClaim(disabilityClaimKey);
        };

        private It should_use_the_correct_statement_sql_statement = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetEmailAddressForUserWhoApprovedDisabilityClaimStatement>.IsAnything));
        };

        private It should_return_the_result_from_the_database = () =>
        {
            result.ShouldEqual(approverEmail);
        };
    }
}