using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_checking_if_client_address_belongs_to_applicant : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;
        private static bool result;
        private static int clientAddressKey;
        private static int applicationNumber;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
            clientAddressKey = 1231;
            applicationNumber = 342;
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<int>(Param.IsAny<DoesClientAddressBelongToApplicantStatement>())).Return(new int[] { clientAddressKey });
        };

        private Because of = () =>
        {
            result = applicantDataManager.DoesClientAddressBelongToApplicant(clientAddressKey, applicationNumber);
        };

        private It should_if_check_if_client_address_belongs_to_client = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<int>(Arg.Is<DoesClientAddressBelongToApplicantStatement>(
                y => y.ApplicationNumber == applicationNumber
                && y.ClientAddressKey == clientAddressKey)));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}