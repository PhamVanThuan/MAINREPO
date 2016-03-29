using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_checking_a_non_existing_application_debit_order_bank_acc : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;
        private static int applicationNumber, bankAccountKey;
        private static bool result;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
            bankAccountKey = 1000;
            applicationNumber = 173;

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<DoesBankAccountBelongToApplicantOnApplicationStatement>()))
                .Return(0);
        };

        private Because of = () =>
        {
            result = applicantDataManager.DoesBankAccountBelongToApplicantOnApplication(applicationNumber, bankAccountKey);
        };

        private It should_check_if_the_bank_account_is_linked_to = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Param.IsAny<DoesBankAccountBelongToApplicantOnApplicationStatement>()));
        };

        private It should_return_an_application_debit_order_bank_account = () =>
        {
            result.ShouldBeFalse();
        };
    }
}