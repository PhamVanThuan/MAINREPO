using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AddressModelManagerSpecs
{
    public class when_populating_the_residential_address_for_a_property : WithCoreFakes
    {
        private static AddressModelManager modelManager;
        private static Property comcorpProperty;
        private static ResidentialAddressModel address;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            comcorpProperty = IntegrationServiceTestHelper.PopulateComcorpPropertyModel(true, true);
            validationUtils = new ValidationUtils();
            modelManager = new AddressModelManager(validationUtils);
        };

        private Because of = () =>
        {
            address = modelManager.PopulateResidentialAddressFromComcorpProperty(comcorpProperty);
        };

        private It should_map_the_street_number_to_the_streetNo_field = () =>
        {
            address.StreetNumber.ShouldEqual(comcorpProperty.StreetNo);
        };

        private It should_map_the_suburb_to_the_addressSuburb_field = () =>
        {
            address.Suburb.ShouldEqual(comcorpProperty.AddressSuburb);
        };

        private It should_map_the_city_to_the_address_city_field = () =>
        {
            address.City.ShouldEqual(comcorpProperty.AddressCity);
        };

        private It should_map_the_province_field = () =>
        {
            address.Province.ShouldEqual(validationUtils.MapComcorpToSAHLProvince(comcorpProperty.Province));
        };

        private It should_map_the_postal_code_field = () =>
        {
            address.PostalCode.ShouldEqual(comcorpProperty.PostalCode);
        };

        private It should_set_the_country_to_south_africa = () =>
        {
            address.Country.ShouldEqual("South Africa");
        };

    }
}
