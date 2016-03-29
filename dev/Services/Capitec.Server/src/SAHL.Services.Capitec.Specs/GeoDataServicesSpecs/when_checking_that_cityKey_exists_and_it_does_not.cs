using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoDataServicesSpecs
{
    public class when_checking_that_cityKey_exists_and_it_does_not : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static int sahlCityKey;
        private static CityDataModel city;
        private static Guid cityGuid;
        private static bool result;

        private Establish context = () =>
        {
            cityGuid = new Guid();
            city = null;
            sahlCityKey = 1;
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesSAHLCityKeyExistQuery>())).Return(city);
        };

        private Because of = () =>
        {
            result = service.DoesSAHLCityKeyExist(sahlCityKey);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}