using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_removing_all_application_fees : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static int ApplicationNumber;

        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            ApplicationNumber = 1;
        };

        private Because of = () =>
        {
            financialDataManager.RemoveAllApplicationFees(ApplicationNumber);
        };

        private It should_delete_the_applications_origination_fee_expenses = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Delete<int>(Arg.Is<RemoveAllApplicationFeesStatement>(y => y.ApplicationNumber == ApplicationNumber)));
        };
    }
}