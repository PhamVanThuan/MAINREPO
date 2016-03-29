using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoDataServicesSpecs
{
    public class when_changing_city_details : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid Id;
        private static Guid provinceId;
        private static int sahlCityKey;
        private static string cityName;

        private Establish context = () =>
        {
            Id = new Guid();
            provinceId = new Guid();
            sahlCityKey = 1;
            cityName = "TestCity";
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
        };

        private Because of = () =>
        {
            service.ChangeCityDetails(Id, cityName, sahlCityKey, provinceId);
        };

        private It should_change_province_details_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<CityDataModel>(Arg.Is<ChangeCityDetailsQuery>(y => y.Id == Id && y.CityName == cityName && y.SAHLCityKey == sahlCityKey && y.ProvinceId == provinceId)));
        };
    }
}