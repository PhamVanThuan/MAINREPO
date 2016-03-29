using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;
using SAHL.Services.Interfaces.FinancialDomain.Models;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_getting_application_fee_attributes : WithCoreFakes
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
            financialDataManager.GetFeeApplicationAttributes(ApplicationNumber);
        };

        private It should_query_for_all_the_applications_attributes = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<FeeApplicationAttributesModel>(Arg.Is<GetFeeApplicationAttributesStatement>(y => y.ApplicationNumber == ApplicationNumber)));
        };
    }
}