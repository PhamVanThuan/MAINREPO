using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Testing.Providers;
using SAHL.Services.AddressDomain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    public class when_getting_suburbKey_for_suburb_city_postalCode_and_province : WithFakes
    {
        private static AddressDataManager addressDataService;
        private static ILinkedKeyManager linkedKeyManager;
        private static Guid addressGuid;
        private static string suburb, city, postalCode, province;
        private static IEnumerable<SuburbDataModel> suburbResult;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<SuburbDataModel> suburbDataModels;

        private Establish context = () =>
        {
            suburb = "la lucia";
            city = "Umhlanga";
            province = "Kwazulu-natal";
            postalCode = "4051";
            addressGuid = CombGuid.Instance.Generate();

            linkedKeyManager = An<ILinkedKeyManager>();
            dbFactory = new FakeDbFactory();
            addressDataService = new AddressDataManager(dbFactory);
            suburbDataModels = new SuburbDataModel[] { new SuburbDataModel(1234, "test", 1111, "1234") };

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<SuburbDataModel>(Param.IsAny<ISqlStatement<SuburbDataModel>>())).Return(suburbDataModels);
        };

        private Because of = () =>
        {
            suburbResult = addressDataService.GetSuburbForModelData(suburb, city, postalCode, province);
        };

        private It should_find_suburbKey_from_the_system = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<SuburbDataModel>(Param.IsAny<ISqlStatement<SuburbDataModel>>()));
        };

        private It should_get_a_matching_suburbKey_from_the_system = () =>
        {
            suburbResult.First().SuburbKey.ShouldEqual(suburbDataModels.First().SuburbKey);
        };
    }
}