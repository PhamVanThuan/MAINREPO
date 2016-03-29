using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_the_external_originator_attribute : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;

        private static OfferAttributeDataModel offerAttribute;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);

            offerAttribute = new OfferAttributeDataModel(1, (int)SAHL.Core.BusinessModel.Enums.OfferAttributeType.ComcorpLoan);
        };

        private Because of = () =>
        {
            applicationDataManager.SaveExternalOriginatorAttribute(offerAttribute);
        };

        private It should_insert_the_offer_attribute = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferAttributeDataModel>(offerAttribute));
        };
    }
}