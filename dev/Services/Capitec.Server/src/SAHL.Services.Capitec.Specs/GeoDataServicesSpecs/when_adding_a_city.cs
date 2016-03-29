using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Geo;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.GeoDataServicesSpecs
{
    public class when_adding_a_city : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid provinceId;
        private static int sahlCityKey;
        private static string cityName;

        private Establish context = () =>
        {
            provinceId = new Guid();
            sahlCityKey = 1;
            cityName = "TestCity";
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
        };

        private Because of = () =>
        {
            service.AddCity(cityName, sahlCityKey, provinceId);
        };

        private It should_add_a_province_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<CityDataModel>(y => y.CityName == cityName && y.SAHLCityKey == sahlCityKey && y.ProvinceId == provinceId)));
        };
    }
}