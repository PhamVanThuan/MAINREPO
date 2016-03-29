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
    public class when_adding_a_province : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid countryId;
        private static int sahlProvinceKey;
        private static string provinceName;

        private Establish context = () =>
        {
            countryId = new Guid();
            sahlProvinceKey = 1;
            provinceName = "TestProvince";
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
        };

        private Because of = () =>
        {
            service.AddProvince(provinceName, sahlProvinceKey, countryId);
        };

        private It should_add_a_province_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ProvinceDataModel>(y => y.ProvinceName == provinceName && y.SAHLProvinceKey == sahlProvinceKey && y.CountryId == countryId)));
        };
    }
}