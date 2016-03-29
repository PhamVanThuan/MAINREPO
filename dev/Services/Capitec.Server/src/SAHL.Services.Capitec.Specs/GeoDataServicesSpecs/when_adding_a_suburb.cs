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
    public class when_adding_a_suburb : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid cityId;
        private static int sahlSuburbKey;
        private static string suburbName;
        private static string postalCode;

        private Establish context = () =>
        {
            cityId = new Guid();
            sahlSuburbKey = 1;
            suburbName = "TestSuburb";
            postalCode = "1234";
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
        };

        private Because of = () =>
        {
            service.AddSuburb(suburbName, sahlSuburbKey, postalCode, cityId);
        };

        private It should_add_a_province_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<SuburbDataModel>(y => y.SAHLSuburbKey == sahlSuburbKey && y.SuburbName == suburbName && y.PostalCode == postalCode && y.CityId == cityId)));
        };
    }
}