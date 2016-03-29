using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_an_application_information_variable_loan : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static OfferInformationVariableLoanDataModel offerInformationVariableLoan;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            offerInformationVariableLoan = new OfferInformationVariableLoanDataModel(1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 
                null, null, null, null, null, null, null, null, null, null, null, null);

        };

        private Because of = () =>
        {
            applicationDataManager.SaveApplicationInformationVariableLoan(offerInformationVariableLoan);
        };

        private It should_insert_a_new_offer_information_variable_loan_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferInformationVariableLoanDataModel>(offerInformationVariableLoan));
        };
    }
}