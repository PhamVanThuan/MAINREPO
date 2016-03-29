using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    public class when_finding_a_post_office_for_a_postal_address : WithFakes
    {
        private static IAddressDataManager addressDataManager;
        private static PostalAddressModel postalAddress;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<PostOfficeDataModel> result;
        private static IEnumerable<PostOfficeDataModel> postOfficeDataModels;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            addressDataManager = new AddressDataManager(dbFactory);
            postalAddress = new PostalAddressModel("12", "", "Hillcrest", "Kwazulu-Natal", "Hillcrest", "3650", AddressFormat.Box);
            postOfficeDataModels = new PostOfficeDataModel[] { new PostOfficeDataModel(1234, "Test", "1234", null) };
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<PostOfficeDataModel>(Param.IsAny<ISqlStatement<PostOfficeDataModel>>())).Return(postOfficeDataModels);
        };

        private Because of = () =>
        {
            result = addressDataManager.GetPostOfficeForModelData(postalAddress.Province, postalAddress.City, postalAddress.PostalCode);
        };

        private It should_query_for_post_office_using_address_data_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<PostOfficeDataModel>(Param.IsAny<ISqlStatement<PostOfficeDataModel>>()));
        };

        private It should_return_the_post_office_key_from_query_result = () =>
        {
            result.First().PostOfficeKey.ShouldEqual(postOfficeDataModels.First().PostOfficeKey);
        };
    }
}