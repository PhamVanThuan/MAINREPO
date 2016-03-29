using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Services.Capitec.Managers.Geo;
using System;

namespace SAHL.Services.Capitec.Specs.GeoManagerSpecs
{
    public class when_changing_province_details_and_the_countryid_is_invalid : WithFakes
    {
        private static IGeoManager geoManager;
        private static IGeoDataManager geoDataServices;
        private static string provinceName;
        private static int sahlProvinceKey;
        private static Guid id;
        private static Guid countryId;
        private static Exception exception;

        public Establish context = () =>
        {
            provinceName = "provinceName";
            id = CombGuid.Instance.Generate();
            geoDataServices = An<IGeoDataManager>();
            geoDataServices.WhenToldTo(x => x.HasSAHLProvinceKeyChanged(id, sahlProvinceKey)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesProvinceIdExist(id)).Return(true);
            geoDataServices.WhenToldTo(x => x.DoesCountryIdExist(Param.IsAny<Guid>())).Return(false);
            geoManager = new GeoManager(geoDataServices);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => geoManager.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId));
        };

        private It should_throw_an_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(Exception));
        };

        private It should_throw_an_exception_with_a_custome_message = () =>
        {
            exception.Message.ShouldEqual("Invalid Country Id");
        };

        private It should_not_change_province_name = () =>
        {
            geoDataServices.WasNotToldTo(x => x.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId));
        };
    }
}
