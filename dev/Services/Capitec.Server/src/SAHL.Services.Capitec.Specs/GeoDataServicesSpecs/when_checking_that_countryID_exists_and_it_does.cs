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
    public class when_checking_that_countryID_exists_and_it_does : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid Id;
        private static bool result;
        private static CountryDataModel country;

        private Establish context = () =>
        {
            country = new CountryDataModel(1, "South Africa");
            Id = Guid.NewGuid();
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesCountryIdExistQuery>())).Return(country);
        };

        private Because of = () =>
        {
            result = service.DoesCountryIdExist(Id);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}