using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_saving_offerinformation_variableloan : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static FakeDbFactory dbFactory;
        private static OfferInformationVariableLoanDataModel offerInformationVariableLoan;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            offerInformationVariableLoan = new OfferInformationVariableLoanDataModel(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        };

        private Because of = () =>
        {
            financialDataManager.SaveOfferInformationVariableLoan(offerInformationVariableLoan);
        };

        private It should_update_offer_information_variable_loan = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update(offerInformationVariableLoan));
        };
    }
}