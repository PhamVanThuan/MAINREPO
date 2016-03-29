using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.CheckClientIsAnApplicantOnTheApplication
{
    public class when_the_client_is_an_applicant_on_the_application : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;
        private static bool result;
        private static int clientKey;
        private static int applicationNumber;

        private Establish context = () =>
        {
            clientKey = 1234567;
            applicationNumber = 7654321;
            result = false;

            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<CheckClientIsAnApplicantOnTheApplicationStatement>())).Return(() => { return 1; });
        };

        private Because of = () =>
        {
            result = applicantDataManager.CheckClientIsAnApplicantOnTheApplication(clientKey, applicationNumber);
        };

        private It should_select_offer_role_date_for_a_given_applicant_on_an_application = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Arg.Is<CheckClientIsAnApplicantOnTheApplicationStatement>(y => y.ApplicationNumber == applicationNumber 
                && y.ClientKey == clientKey)));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}