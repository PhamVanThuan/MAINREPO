using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.PropertyDomain.Managers;
using System;

namespace SAHL.PropertyDomainService.Specs.ManagerSpecs
{
    internal class when_updateing_a_comcorp_offer_property_details : WithFakes
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
                string.Empty, string.Empty,string.Empty, DateTime.Now, DateTime.Now);
        };

        private Because of = () =>
        {
            propertyDataManager.UpdateComcorpOfferPropertyDetails(comcorpOfferPropertyDetailsDataModel);
        };

        private It should_save_the_property = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<ComcorpOfferPropertyDetailsDataModel>(comcorpOfferPropertyDetailsDataModel));
        };
    }
}