using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.ChangeSuburbDetails")]
    public class when_changing_suburbs_details_where_sahlkey_changed : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static Guid id;
        private static string suburbName;
        private static int sahlSuburbKey;
        private static string postalCode;
        private static Guid cityId;

        private Establish context = () =>
        {
            suburbName = "suburbName";
            sahlSuburbKey = 0;
            postalCode = "0000";
            cityId = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.DoesSuburbIdExist(id)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesCityIdExist(cityId)).Return(true);
            geoDataServices.WhenToldTo(x => x.HasSAHLSuburbKeyChanged(id, sahlSuburbKey)).Return(true);
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            geoManager.ChangeSuburbDetails(id, suburbName, sahlSuburbKey, postalCode, cityId);
        };

        private It should_check_if_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSuburbIdExist(id));
        };

        private It should_check_if_sahl_key_changed = () =>
        {
            geoDataServices.WasToldTo(x => x.HasSAHLSuburbKeyChanged(id, sahlSuburbKey));
        };

        private It should_check_if_sahl_key_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSAHLSuburbKeyExist(sahlSuburbKey));
        };

        private It should_check_if_city_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesCityIdExist(cityId));
        };

        private It should_change_suburbs_details = () =>
        {
            geoDataServices.WasToldTo(x => x.ChangeSuburbsDetails(id, suburbName, sahlSuburbKey, postalCode, cityId));
        };
    }
}