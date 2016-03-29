using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Domicilium
{
    internal class when_applicant_does_not_have_a_pending_domicilium : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;
        private static int clientKey, expectedResult, clientDomiciliumKey;
        private static bool response;

        private Establish context = () =>
        {
            clientKey = 50;
            clientDomiciliumKey = 98;
            expectedResult = 0;
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<DoesApplicantHavePendingDomiciliumOnApplicationStatement>())).Return(() => { return expectedResult; });
        };

        private Because of = () =>
        {
            response = applicantDataManager.DoesApplicantHavePendingDomiciliumOnApplication(clientKey, clientDomiciliumKey);
        };

        private It should_check_if_domicilium_address_is_pending_domicilium_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Param.IsAny<DoesApplicantHavePendingDomiciliumOnApplicationStatement>()));
        };

        private It should_confirm_that_domcilium_address_is_not_pending_domicilium_address = () =>
        {
            response.ShouldBeFalse();
        };
    }
}