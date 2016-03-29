using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.ChangeSuburbDetails")]
    public class when_changing_suburbs_details_where_no_name_supplied : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static Guid id;
        private static string suburbName;
        private static int sahlSuburbKey;
        private static string postalCode;
        private static Guid cityId;
        private static Exception exception;

        private Establish context = () =>
        {
            geoDataServices = An<IGeoDataManager>();
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.ChangeSuburbDetails(id, suburbName, sahlSuburbKey, postalCode, cityId));
        };

        private It should_not_check_if_id_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesSuburbIdExist(id));
        };

        private It should_not_check_if_sahl_key_changed = () =>
        {
            geoDataServices.WasNotToldTo(x => x.HasSAHLSuburbKeyChanged(id, sahlSuburbKey));
        };

        private It should_not_check_if_sahl_key_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesSAHLSuburbKeyExist(sahlSuburbKey));
        };

        private It should_not_check_if_city_id_exists = () =>
        {
            geoDataServices.WasNotToldTo(x => x.DoesCityIdExist(cityId));
        };

        private It should_not_change_suburbs_details = () =>
        {
            geoDataServices.WasNotToldTo(x => x.ChangeSuburbsDetails(id, suburbName, sahlSuburbKey, postalCode, cityId));
        };
    }
}