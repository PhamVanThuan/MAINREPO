using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoDataServicesSpecs
{
    public class when_changing_suburb_details : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid Id;
        private static Guid cityId;
        private static int sahlSuburbKey;
        private static string suburbName;
        private static string postalCode;

        private Establish context = () =>
        {
            Id = new Guid();
            cityId = new Guid();
            sahlSuburbKey = 1;
            suburbName = "TestSuburb";
            postalCode = "1234";

            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
        };

        private Because of = () =>
        {
            service.ChangeSuburbsDetails(Id, suburbName, sahlSuburbKey, postalCode, cityId);
        };

        private It should_change_province_details_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<SuburbDataModel>(Arg.Is<ChangeSuburbsDetailsQuery>(y => y.Id == Id && y.SuburbName == suburbName && y.SAHLSuburbKey == sahlSuburbKey && y.CityId == cityId)));
        };
    }
}