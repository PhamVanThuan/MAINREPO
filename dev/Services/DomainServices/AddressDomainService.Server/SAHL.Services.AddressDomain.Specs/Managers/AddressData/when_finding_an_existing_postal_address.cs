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
    public class when_finding_an_existing_postal_address : WithFakes
    {
        private static AddressDataManager addressDataManager;
        private static PostalAddressModel postalAddress;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<AddressDataModel> existingPostalAddresses;
        private static IEnumerable<AddressDataModel> results;

        private Establish context = () =>
        {
            existingPostalAddresses = new AddressDataModel[] { new AddressDataModel(2, "1883", "", "", "", "", "", null, 1234, "South Africa", "Gauteng",
                "Sandton", "Sandton", "1234", "", null, null, null, null, null, null, null)};
            dbFactory = new FakeDbFactory();
            addressDataManager = new AddressDataManager(dbFactory);
            postalAddress = new PostalAddressModel("1883", "", "Hillcrest", "Kwazulu Natal", "Hillcrest", "3650", AddressFormat.Box);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<AddressDataModel>(Param.IsAny<ISqlStatement<AddressDataModel>>()))
                .Return(existingPostalAddresses);
        };

        private Because of = () =>
        {
            results = addressDataManager.FindPostalAddressFromAddressValues(postalAddress);
        };

        private It should_query_for_an_existing_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<AddressDataModel>(Param.IsAny<ISqlStatement<AddressDataModel>>()));
        };

        private It should_return_the_key_for_the_existing_address = () =>
        {
            results.First().AddressKey.ShouldEqual(existingPostalAddresses.First().AddressKey);
        };
    }
}