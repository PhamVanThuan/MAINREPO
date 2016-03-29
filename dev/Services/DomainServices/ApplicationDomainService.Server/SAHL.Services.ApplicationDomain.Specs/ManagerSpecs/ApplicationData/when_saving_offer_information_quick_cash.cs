using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_offer_information_quick_cash : WithCoreFakes
    {
        private static ApplicationDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static OfferInformationQuickCashDataModel offerInformationQuickCashDataModel;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            offerInformationQuickCashDataModel = new OfferInformationQuickCashDataModel(1, 5000, 12, 5000);
            dataManager = new ApplicationDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.SaveOfferInformationQuickCash(offerInformationQuickCashDataModel);
        };

        private It should_insert_the_offer_information_record = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(offerInformationQuickCashDataModel));
        };

        private It should_complete = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}