using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.PropertyDataModelManagerSpecs
{
    public class when_populating_comcorp_property_data : WithCoreFakes
    {
        private static PropertyDataModelManager modelManager;
        private static Property comcorpProperty;
        private static ComcorpApplicationPropertyDetailsModel comcorpPropertyDetailsModel;
        private static string occupancyType, propertyType, titleType, sectionalUnitNumber, nameRegistered;

        private Establish context = () =>
        {
            occupancyType = "Owner Occupied";
            propertyType = "House";
            titleType = "Freehold";
            sectionalUnitNumber = "23";
            nameRegistered = "Wilbur Speed";
            comcorpProperty = IntegrationServiceTestHelper.PopulateComcorpPropertyModel(true, true);
            modelManager = new PropertyDataModelManager();
        };

        private Because of = () =>
        {
            comcorpPropertyDetailsModel = modelManager.PopulateComcorpPropertyDetails(occupancyType, propertyType, titleType,
                sectionalUnitNumber, nameRegistered, comcorpProperty);
        };

        private It should_map_the_street_number_to_the_streetNo_field = () =>
        {
            comcorpPropertyDetailsModel.StreetNo.ShouldEqual(comcorpProperty.StreetNo);
        };

        private It should_map_the_suburb_to_the_addressSuburb_field = () =>
        {
            comcorpPropertyDetailsModel.Suburb.ShouldEqual(comcorpProperty.AddressSuburb);
        };

        private It should_map_the_city_to_the_address_city_field = () =>
        {
            comcorpPropertyDetailsModel.City.ShouldEqual(comcorpProperty.AddressCity);
        };

        private It should_map_the_province_field = () =>
        {
            comcorpPropertyDetailsModel.Province.ShouldEqual(comcorpProperty.Province);
        };

        private It should_map_the_street_name = () =>
        {
            comcorpPropertyDetailsModel.StreetName.ShouldEqual(comcorpProperty.StreetName);
        };

        private It should_map_the_postal_code_field = () =>
        {
            comcorpPropertyDetailsModel.PostalCode.ShouldEqual(comcorpProperty.PostalCode);
        };

        private It should_map_the_erf_number_to_the_StandErfNo_field = () =>
        {
            comcorpPropertyDetailsModel.StandErfNo.ShouldEqual(comcorpProperty.StandErfNo);
        };

        private It should_map_the_erf_portion_number_to_the_portionNo_field = () =>
        {
            comcorpPropertyDetailsModel.PortionNo.ShouldEqual(comcorpProperty.PortionNo);
        };

        private It should_map_the_inspection_contact_detail = () =>
        {
            comcorpPropertyDetailsModel.ContactName.ShouldEqual(comcorpProperty.ContactName);
        };

        private It should_map_the_inspection_contact_number = () =>
        {
            comcorpPropertyDetailsModel.ContactCellphone.ShouldEqual(comcorpProperty.ContactCellphone);
        };

        private It should_use_the_occupancy_type_that_was_provided = () =>
        {
            comcorpPropertyDetailsModel.SAHLOccupancyType.ShouldEqual(occupancyType);
        };

        private It should_use_the_title_type_that_was_provided = () =>
        {
            comcorpPropertyDetailsModel.SAHLTitleType.ShouldEqual(titleType);
        };

        private It should_use_the_property_type_that_was_provided = () =>
        {
            comcorpPropertyDetailsModel.SAHLPropertyType.ShouldEqual(propertyType);
        };

        private It should_use_the_sectional_unit_number_that_was_provided = () =>
        {
            comcorpPropertyDetailsModel.SectionalTitleUnitNo.ShouldEqual(sectionalUnitNumber);
        };

        private It should_use_the_property_name_registered_that_was_provided = () =>
        {
            comcorpPropertyDetailsModel.NamePropertyRegistered.ShouldEqual(nameRegistered);
        };

        private It should_map_the_complex_name_field = () =>
        {
            comcorpPropertyDetailsModel.ComplexName.ShouldEqual(comcorpProperty.ComplexName);
        };
    }
}