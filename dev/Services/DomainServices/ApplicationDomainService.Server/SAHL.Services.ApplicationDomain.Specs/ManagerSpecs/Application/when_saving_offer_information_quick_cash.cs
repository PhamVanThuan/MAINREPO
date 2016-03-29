using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_saving_offer_information_quick_cash : WithFakes
    {
        private static IApplicationDataManager dataManager;
        private static ApplicationManager manager;
        private static int offerInformationKey;
        private static int term;

        private Establish context = () =>
        {
            dataManager = An<IApplicationDataManager>();
            manager = new ApplicationManager(dataManager);

            offerInformationKey = 11231;
            term = 24;
        };

        private Because of = () =>
        {
            manager.SaveApplicationInformationQuickCash(offerInformationKey);
        };

        private It should_save_offer_information_quick_cash = () =>
        {
            dataManager.WasToldTo(x => x.SaveOfferInformationQuickCash(Param<OfferInformationQuickCashDataModel>.Matches(m =>
                m.OfferInformationKey == offerInformationKey)));
        };
    }
}