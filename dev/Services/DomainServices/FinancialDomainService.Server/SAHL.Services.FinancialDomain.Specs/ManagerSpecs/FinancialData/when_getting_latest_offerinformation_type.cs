using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_getting_latest_offerinformation_type : WithCoreFakes
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
            financialDataManager.GetLatestOfferInformationType(ApplicationNumber);
        };

        private It should_query_for_the_latest_offer_information_type = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Arg.Is<GetLatestOfferInformationTypeStatement>(y => y.ApplicationNumber == ApplicationNumber)));
        };
    }
}