using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.AddProvince")]
    public class when_adding_a_province : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static string provinceName;
        private static int sahlProvinceKey;
        private static Guid countryId;

        private Establish context = () =>
        {
            provinceName = "provinceName";
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.DoesProvinceNameExist(provinceName)).Return(false);
            geoDataServices.WhenToldTo(x => x.DoesSAHLProvinceKeyExist(sahlProvinceKey)).Return(false);
            geoDataServices.WhenToldTo(x => x.DoesCountryIdExist(countryId)).Return(true);
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            geoManager.AddProvince(provinceName, sahlProvinceKey, countryId);
        };

        private It province_name_should_not_be_changed = () =>
        {
            provinceName.ShouldEqual(provinceName);
        };

        public It should_check_that_the_country_id_is_valid = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesCountryIdExist(countryId));
        };

        private It should_check_if_name_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesProvinceNameExist(provinceName));
        };

        private It should_check_sahl_province_key_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSAHLProvinceKeyExist(sahlProvinceKey));
        };

        private It should_add_province = () =>
        {
            geoDataServices.WasToldTo(x => x.AddProvince(provinceName, sahlProvinceKey, countryId));
        };
    }
}