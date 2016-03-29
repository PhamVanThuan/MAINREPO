using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_getting_latest_offerinformation : WithCoreFakes
    {
        private static FinancialDataManager _financialDataManager;
        private static int _applicationNumber;

        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            _financialDataManager = new FinancialDataManager(dbFactory);
            _applicationNumber = 1;
        };

        private Because of = () =>
        {
            _financialDataManager.GetLatestApplicationOfferInformation(_applicationNumber);
        };

        private It should_query_for_the_latest_offer_information_type = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<OfferInformationDataModel>(Arg.Is<GetLatestApplicationOfferInformationStatement>(y => y.ApplicationNumber == _applicationNumber)));
        };
    }
}