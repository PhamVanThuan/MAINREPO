using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.AddCity")]
    public class when_adding_a_city_where_no_name_supplied : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static string cityName;
        private static int sahlCityKey;
        private static Guid provinceId;
        private static Exception exception;

        private Establish context = () =>
        {
            cityName = string.Empty;
            geoDataServices = An<IGeoDataManager>();
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.AddCity(cityName, sahlCityKey, provinceId));
        };

        private It should_check_if_sahl_city_key_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesSAHLCityKeyExist(sahlCityKey));
        };

        private It should_check_if_province_id_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesProvinceIdExist(provinceId));
        };

        private It should_add_city = () =>
        {
            geoDataServices.WasNotToldTo(x => x.AddCity(cityName, sahlCityKey, provinceId));
        };
    }
}