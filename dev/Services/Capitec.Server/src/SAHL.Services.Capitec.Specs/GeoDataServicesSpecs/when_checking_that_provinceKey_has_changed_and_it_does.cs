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
    public class when_checking_that_provinceKey_has_changed_and_it_does : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid Id;
        private static bool result;
        private static ProvinceDataModel province;
        private static Guid countryGuid;
        private static int sahlProvinceKey;

        private Establish context = () =>
        {
            countryGuid = new Guid();
            Id = new Guid();
            sahlProvinceKey = 1;
            province = null;
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<HasSAHLProvinceKeyChangedQuery>())).Return(province);
        };

        private Because of = () =>
        {
            result = service.HasSAHLProvinceKeyChanged(Id, sahlProvinceKey);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeTrue();
        };
    }
}