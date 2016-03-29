using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.ChangeCityDetails")]
    public class when_changing_city_details_where_sahlkey_invalid : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static Guid id;
        private static string cityName;
        private static int sahlCityKey;
        private static Guid provinceId;
        private static Exception exception;

        private Establish context = () =>
        {
            id = CombGuid.Instance.Generate();
            cityName = "testName";
            sahlCityKey = 0;
            provinceId = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.DoesCityIdExist(id)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesProvinceIdExist(provinceId)).Return(true);
            geoDataServices.WhenToldTo(x => x.HasSAHLCityKeyChanged(id, sahlCityKey)).Return(true);
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.ChangeCityDetails(id, cityName, sahlCityKey, provinceId));
        };

        private It should_check_if_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesCityIdExist(id));
        };

        private It should_check_if_sahl_key_changed = () =>
        {
            geoDataServices.WasToldTo(x => x.HasSAHLCityKeyChanged(id, sahlCityKey));
        };

        private It should_check_if_sahl_key_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSAHLCityKeyExist(sahlCityKey));
        };

        private It should_check_if_province_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesProvinceIdExist(provinceId));
        };

        private It should_change_city_details = () =>
        {
            geoDataServices.WasToldTo(x => x.ChangeCityDetails(id, cityName, sahlCityKey, provinceId));
        };
    }
}