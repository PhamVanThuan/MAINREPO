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
    public class when_checking_that_suburbKey_exists_and_it_does : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;    
        private static bool result;
        private static SuburbDataModel suburb;
        private static int sahlSuburbKey;

        private Establish context = () =>
        {
          
            sahlSuburbKey = 1;
            suburb = null;
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesSAHLSuburbKeyExistQuery>())).Return(suburb);
        };

        private Because of = () =>
        {
            result = service.DoesSAHLSuburbKeyExist(sahlSuburbKey);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}