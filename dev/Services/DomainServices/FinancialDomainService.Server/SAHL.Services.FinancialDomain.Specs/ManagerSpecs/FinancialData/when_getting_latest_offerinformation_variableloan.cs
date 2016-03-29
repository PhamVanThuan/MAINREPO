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
    public class when_getting_latest_offerinformation_variableloan : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static int applicationInformationKey;

        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            applicationInformationKey = 1;
        };

        private Because of = () =>
        {
            financialDataManager.GetApplicationInformationVariableLoan(applicationInformationKey);
        };

        private It should_query_for_the_latest_offer_information_variable_loan = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<OfferInformationVariableLoanDataModel>(Arg.Is<GetOfferInformationVariableLoanStatement>(y => y.ApplicationInformationKey == applicationInformationKey)));
        };
    }
}