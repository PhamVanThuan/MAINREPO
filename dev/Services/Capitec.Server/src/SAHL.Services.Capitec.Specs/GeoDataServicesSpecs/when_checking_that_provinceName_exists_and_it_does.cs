using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Geo;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.GeoDataServicesSpecs
{
    public class when_checking_that_provinceName_exists_and_it_does : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static string provinceName;
        private static ProvinceDataModel province;
        private static Guid countryGuid;
        private static bool result;

        private Establish context = () =>
        {
            countryGuid = new Guid();
            province = new ProvinceDataModel(1, "TestProvince", countryGuid);
            provinceName = "TestProvince";
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesProvinceNameExistsQuery>())).Return(province);
        };

        private Because of = () =>
        {
            result = service.DoesProvinceNameExist(provinceName);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}