using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.AddProvince")]
    public class when_adding_a_province_where_province_exists : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static string provinceName;
        private static int sahlProvinceKey;
        private static Guid countryId;
        private static Exception exception;

        private Establish context = () =>
        {
            provinceName = "provinceName";
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.DoesProvinceNameExist(provinceName)).Return(true);
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.AddProvince(provinceName, sahlProvinceKey, countryId));
        };

        private It province_name_should_not_be_changed = () =>
        {
            provinceName.ShouldEqual(provinceName);
        };

        private It should_check_if_name_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesProvinceNameExist(provinceName));
        };

        private It should_not_check_sahl_province_key_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesSAHLProvinceKeyExist(sahlProvinceKey));
        };

        private It should_add_province = () =>
        {
            geoDataServices.WasNotToldTo(x => x.AddProvince(provinceName, sahlProvinceKey, countryId));
        };
    }
}