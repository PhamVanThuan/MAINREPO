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
    public class when_checking_that_provinceID_exists_and_it_does_not : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid Id, countryGuid;
        private static bool result;
        private static ProvinceDataModel province;

        private Establish context = () =>
        {
            countryGuid = new Guid();
            province = null;
            Id = new Guid();
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesProvinceIdExistQuery>())).Return(province);
        };

        private Because of = () =>
        {
            result = service.DoesProvinceIdExist(Id);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}