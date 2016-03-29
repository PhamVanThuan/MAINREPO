using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AddressModelManagerSpecs
{
    public class when_populating_the_property_address : WithCoreFakes
    {
        private static AddressModelManager modelManager;
        private static Property comcorpProperty;
        private static PropertyAddressModel propertyAddress;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            comcorpProperty = IntegrationServiceTestHelper.PopulateComcorpPropertyModel(true, true);
            validationUtils = new ValidationUtils();
            modelManager = new AddressModelManager(validationUtils);
        };

        private Because of = () =>
        {
            propertyAddress = modelManager.PopulatePropertyAddressFromComcorpProperty(comcorpProperty);
        };

        private It should_map_the_street_number_to_the_streetNo_field = () =>
        {
            propertyAddress.StreetNumber.ShouldEqual(comcorpProperty.StreetNo);
        };

        private It should_map_the_suburb_to_the_addressSuburb_field = () =>
        {
            propertyAddress.Suburb.ShouldEqual(comcorpProperty.AddressSuburb);
        };

        private It should_map_the_city_to_the_address_city_field = () =>
        {
            propertyAddress.City.ShouldEqual(comcorpProperty.AddressCity);
        };

        private It should_map_the_province_field = () =>
        {
            propertyAddress.Province.ShouldEqual(validationUtils.MapComcorpToSAHLProvince(comcorpProperty.Province));
        };

        private It should_map_the_postal_code_field = () =>
        {
            propertyAddress.PostalCode.ShouldEqual(comcorpProperty.PostalCode);
        };

        private It should_map_the_erf_number_to_the_StandErfNo_field = () =>
        {
            propertyAddress.ErfNumber.ShouldEqual(comcorpProperty.StandErfNo);
        };

        private It should_map_the_erf_portion_number_to_the_portionNo_field = () =>
        {
            propertyAddress.ErfPortionNumber.ShouldEqual(comcorpProperty.PortionNo);
        };
    }
}