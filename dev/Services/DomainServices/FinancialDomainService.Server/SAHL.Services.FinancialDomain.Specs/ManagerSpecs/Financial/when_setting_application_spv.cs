using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.Managers;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.Financial
{
    public class when_setting_application_spv : WithCoreFakes
    {
        private static IFinancialDataManager financialDataManager;
        private static FinancialManager financialManager;

        private static int applicationInformationKey = 99;
        private static int SPVKey = 77;
        private static OfferInformationVariableLoanDataModel offerInformationVariableLoan;

        private Establish context = () =>
        {
            offerInformationVariableLoan = new OfferInformationVariableLoanDataModel(55, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, SPVKey, null, null, null, null, null);

            financialDataManager = An<IFinancialDataManager>();
            financialManager = new FinancialManager(financialDataManager);

            financialDataManager.WhenToldTo(x => x.GetApplicationInformationVariableLoan(applicationInformationKey)).Return(offerInformationVariableLoan);
        };

        private Because of = () =>
        {
            financialManager.SetApplicationInformationSPVKey(applicationInformationKey, SPVKey);
        };

        private It should_get_the_latest_application_information_mortgage_loan = () =>
        {
            financialDataManager.WasToldTo(x => x.GetApplicationInformationVariableLoan(applicationInformationKey));
        };

        private It should_set_the_spv_key_on_the_application_information_mortgage_loan = () =>
        {
            financialDataManager.WasToldTo(x => x.SaveOfferInformationVariableLoan(Arg.Is<OfferInformationVariableLoanDataModel>(m => m.SPVKey == SPVKey)));
        };
    }
}