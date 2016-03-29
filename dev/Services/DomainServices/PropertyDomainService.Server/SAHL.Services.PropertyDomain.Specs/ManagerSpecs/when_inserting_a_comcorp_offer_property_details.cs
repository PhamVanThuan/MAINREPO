using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.PropertyDomain.Managers;
using System;

namespace SAHL.Services.PropertyDomain.Specs.ManagerSpecs
{
    public class when_inserting_a_comcorp_offer_property_details : WithFakes
    {
        private static IPropertyDataManager propertyDataManager;
        private static FakeDbFactory dbFactory;
        private static ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetailsDataModel;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            propertyDataManager = new PropertyDataManager(dbFactory);

            comcorpOfferPropertyDetailsDataModel = new ComcorpOfferPropertyDetailsDataModel(0, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, DateTime.Now, DateTime.Now);
        };

        private Because of = () =>
        {
            propertyDataManager.InsertComcorpOfferPropertyDetails(comcorpOfferPropertyDetailsDataModel);
        };

        private It should_save_the_property = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<ComcorpOfferPropertyDetailsDataModel>(comcorpOfferPropertyDetailsDataModel));
        };
    }
}