using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    public class when_adding_a_province_and_the_countryid_is_invalid : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static string provinceName;
        private static int sahlProvinceKey;
        private static Guid countryId;
        private static Exception exception;

        private Establish context = () =>
        {
            provinceName = "New Province";
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.DoesCountryIdExist(Param.IsAny<Guid>())).Return(false);
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.AddProvince(provinceName, sahlProvinceKey, countryId));
        };

        private It should_throw_an_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(Exception));
        };

        private It should_throw_an_exception_with_a_custom_message = () =>
        {
            exception.Message.ShouldEqual("Invalid Country Id");
        };

        private It should_not_add_a_province = () =>
        {
            geoDataServices.WasNotToldTo(x => x.AddProvince(provinceName, sahlProvinceKey, countryId));
        };
    }
}
