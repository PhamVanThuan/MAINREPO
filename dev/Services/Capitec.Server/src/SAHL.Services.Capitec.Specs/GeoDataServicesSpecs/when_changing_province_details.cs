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
using System.Linq;

namespace SAHL.Services.Capitec.Specs.GeoDataServicesSpecs
{
    public class when_changing_province_details : WithFakes
    {
        private static GeoDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid Id;
        private static Guid countryId;
        private static int sahlProvinceKey;
        private static string provinceName;

        private Establish context = () =>
        {
            Id = new Guid();
            countryId = new Guid();
            sahlProvinceKey = 1;
            provinceName = "TestProvince";
            dbFactory = new FakeDbFactory();
            service = new GeoDataManager(dbFactory);
        };

        private Because of = () =>
        {
            service.ChangeProvinceDetails(Id, provinceName, sahlProvinceKey, countryId);
        };

        private It should_change_province_details_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<ProvinceDataModel>(Arg.Is<ChangeProvinceDetailsQuery>(y => y.ProvinceName == provinceName
                && y.SAHLProvinceKey == sahlProvinceKey && y.CountryId == countryId)));
        };
    }
}