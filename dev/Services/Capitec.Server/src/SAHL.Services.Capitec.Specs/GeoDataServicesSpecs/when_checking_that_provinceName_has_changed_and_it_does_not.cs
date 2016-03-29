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
    public class when_checking_that_provinceName_has_changed_and_it_does_not : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static string provinceName;
        private static ProvinceDataModel province;
        private static Guid countryGuid;
        private static Guid Id;
        private static bool result;

        private Establish context = () =>
        {
            countryGuid = new Guid();
            Id = new Guid();
            province = new ProvinceDataModel(1, "TestProvince", countryGuid);
            provinceName = "TestProvince";
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<HasProvinceNameChangedQuery>())).Return(province);
        };

        private Because of = () =>
        {
            result = service.HasProvinceNameChanged(Id, provinceName);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}