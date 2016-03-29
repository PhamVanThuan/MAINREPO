using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_an_application : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static int expectedApplicationKey, applicationNumber;
        private static OfferDataModel offer;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            expectedApplicationKey = 1234567;
            offer = new OfferDataModel(1, 1, null, null, null, null, null, null, 1, 1, null);

            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<OfferDataModel>(Param.IsAny<OfferDataModel>()))
                .Callback<OfferDataModel>(y => { y.OfferKey = expectedApplicationKey; });
        };

        private Because of = () =>
        {
            applicationNumber = applicationDataManager.SaveApplication(offer);
        };

        private It should_insert_a_new_offer_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferDataModel>(offer));
        };

        private It should_return_the_inserted_offer_key = () =>
        {
            applicationNumber.ShouldEqual(expectedApplicationKey);
        };
    }
}