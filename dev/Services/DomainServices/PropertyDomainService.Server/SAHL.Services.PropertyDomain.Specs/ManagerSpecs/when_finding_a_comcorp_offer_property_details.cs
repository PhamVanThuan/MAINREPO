using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.PropertyDomain.Managers;

namespace SAHL.PropertyDomainService.Specs.ManagerSpecs
{
    public class when_finding_a_comcorp_offer_property_details : WithFakes
    {
        private static IPropertyDataManager propertyDataManager;
        private static FakeDbFactory dbFactory;
        private static int applicationNumber;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            propertyDataManager = new PropertyDataManager(dbFactory);
            applicationNumber = 123;
        };

        private Because of = () =>
        {
            propertyDataManager.FindExistingComcorpOfferPropertyDetails(applicationNumber);
        };

        private It should_save_the_property = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.GetByKey<ComcorpOfferPropertyDetailsDataModel, int>(applicationNumber));
        };
    }
}