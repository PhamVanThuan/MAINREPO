using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.ChangeProvinceDetails")]
    public class when_changing_province_details_where_no_name_supplied : WithFakes
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
            provinceName = string.Empty;
            id = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            geoManager = new GeoManager(geoDataServices);
        };

        public Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId));
        };

        public It should_check_if_name_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesProvinceNameExist(provinceName));
        };

        public It should_check_if_sahl_key_changed = () =>
        {
            geoDataServices.WasNotToldTo(x => x.HasSAHLProvinceKeyChanged(id, sahlProvinceKey));
        };

        public It should_not_check_if_id_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesProvinceIdExist(id));
        };

        public It should_change_province_name = () =>
        {
            geoDataServices.WasNotToldTo(x => x.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId));
        };
    }
}