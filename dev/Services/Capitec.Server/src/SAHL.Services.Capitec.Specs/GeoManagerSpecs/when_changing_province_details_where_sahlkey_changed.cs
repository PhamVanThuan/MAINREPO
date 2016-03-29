using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.ChangeProvinceDetails")]
    public class when_changing_province_details_where_sahlkey_changed : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static string provinceName;
        private static int sahlProvinceKey;
        private static Guid id;
        private static Guid countryId;

        public Establish context = () =>
        {
            provinceName = "provinceName";
            id = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.DoesProvinceIdExist(id)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesProvinceNameExist(provinceName)).Return(false);
            geoDataServices.WhenToldTo(x => x.HasSAHLProvinceKeyChanged(id, sahlProvinceKey)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesSAHLProvinceKeyExist(sahlProvinceKey)).Return(false);
            geoDataServices.WhenToldTo(x => x.DoesCountryIdExist(countryId)).Return(true);
            geoManager = new GeoManager(geoDataServices);
        };

        public Because of = () =>
        {
            geoManager.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId);
        };

        public It should_check_if_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesProvinceIdExist(id));
        };

        public It should_check_that_the_country_id_is_valid = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesCountryIdExist(countryId));
        };

        public It should_not_check_if_name_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesProvinceNameExist(provinceName));
        };

        public It should_check_if_sahl_key_changed = () =>
        {
            geoDataServices.WasToldTo(x => x.HasSAHLProvinceKeyChanged(id, sahlProvinceKey));
        };

        public It should_check_if_sahl_key_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSAHLProvinceKeyExist(sahlProvinceKey));
        };

        public It should_change_province_name = () =>
        {
            geoDataServices.WasToldTo(x => x.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId));
        };
    }
}