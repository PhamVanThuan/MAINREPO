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
    public class when_checking_that_cityID_exists_and_it_does : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid Id, provinceGuid;
        private static bool result;
        private static CityDataModel city;

        private Establish context = () =>
        {
            provinceGuid = new Guid();
            Id = new Guid();
            city = new CityDataModel(1, "TestCity", provinceGuid);
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesCityIdExistQuery>())).Return(city);
        };

        private Because of = () =>
        {
            result = service.DoesCityIdExist(Id);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}