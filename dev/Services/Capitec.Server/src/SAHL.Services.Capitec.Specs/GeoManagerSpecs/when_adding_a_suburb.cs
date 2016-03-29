﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    [Subject("SAHL.Services.Capitec.Services.GeoManager.AddSuburb")]
    public class when_adding_a_suburb : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
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
            geoDataServices.WhenToldTo(x => x.DoesCityIdExist(cityId)).Return(true);
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            geoManager.AddSuburb(suburbName, sahlSuburbKey, postalCode, cityId);
        };

        private It should_check_if_sahl_suburb_key_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesSAHLSuburbKeyExist(sahlSuburbKey));
        };

        private It should_check_if_city_id_exists = () =>
        {
            geoDataServices.WasToldTo(x => x.DoesCityIdExist(cityId));
        };

        private It should_add_a_suburb = () =>
        {
            geoDataServices.WasToldTo(x => x.AddSuburb(suburbName, sahlSuburbKey, postalCode, cityId));
        };
    }
}