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
    public class when_retrieving_rate_config_values : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static int marginKey;
        private static int marketRateKey;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            marginKey = 11;
            marketRateKey = 9;
        };

        private Because of = () =>
        {
            financialDataManager.GetRateConfigurationValues(marginKey, marketRateKey);
        };

        private It should_query_for_the_rate_configuration_values = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<RateConfigurationValuesModel>(Arg.Is<GetRateConfigurationValuesStatement>(y => y.MarginKey == marginKey && y.MarketRateKey == marketRateKey)));
        };
    }
}