using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.ChangeProvinceDetails")]
    public class when_changing_province_details_where_province_exists : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static string provinceName;
        private static int sahlProvinceKey;
        private static Guid id;
        private static Guid countryId;
        private static Exception exception;

        public Establish context = () =>
        {
            provinceName = "provinceName";
            id = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.DoesProvinceIdExist(id)).Return(true);
            geoDataServices.WhenToldTo(x => x.HasProvinceNameChanged(id, provinceName)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesProvinceNameExist(provinceName)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesCountryIdExist(countryId)).Return(true);
            geoManager = new GeoManager(geoDataServices);
        };

        public Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId));
        };

        public It should_not_check_if_name_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesProvinceNameExist(provinceName));
        };

        public It should_check_that_the_country_id_is_valid = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesCountryIdExist(countryId));
        };

        public It should_check_if_sahl_key_changed = () =>
        {
            geoDataServices.WasNotToldTo(x => x.HasSAHLProvinceKeyChanged(id, sahlProvinceKey));
        };

        public It should_not_check_if_sahl_key_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesSAHLProvinceKeyExist(sahlProvinceKey));
        };

        public It should_change_province_name = () =>
        {
            geoDataServices.WasNotToldTo(x => x.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId));
        };
    }
}