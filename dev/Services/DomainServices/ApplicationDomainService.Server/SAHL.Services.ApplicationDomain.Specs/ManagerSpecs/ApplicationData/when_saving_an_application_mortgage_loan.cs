using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_an_application_mortgage_loan : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static OfferMortgageLoanDataModel offerMortgageLoan;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            offerMortgageLoan = new OfferMortgageLoanDataModel(1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, (int)Language.English);
        };

        private Because of = () =>
        {
            applicationDataManager.SaveApplicationMortgageLoan(offerMortgageLoan);
        };

        private It should_insert_a_new_offer_mortgage_loan_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferMortgageLoanDataModel>(offerMortgageLoan));
        };
    }
}