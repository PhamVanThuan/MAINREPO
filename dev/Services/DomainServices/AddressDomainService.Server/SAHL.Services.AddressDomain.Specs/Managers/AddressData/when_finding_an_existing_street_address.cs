using Machine.Fakes;
using Machine.Specifications;
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
    public class when_finding_an_existing_street_address : WithFakes
    {
        private static AddressDataManager addressDataManager;
        private static StreetAddressModel streetAddress;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<AddressDataModel> existingStreetAddresses;
        private static IEnumerable<AddressDataModel> results;

        private Establish context = () =>
        {
            existingStreetAddresses = new AddressDataModel[] { new AddressDataModel(1, null, null, null, null, "7", "Maryland Avenue", 1234, null, "South Africa", "Kwazulu-Natal",
                "Durban North", "Durban", "4051", null, null, null, null, null, null, null, null)};
            dbFactory = new FakeDbFactory();
            addressDataManager = new AddressDataManager(dbFactory);
            streetAddress = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<AddressDataModel>(Param.IsAny<ISqlStatement<AddressDataModel>>()))
                .Return(existingStreetAddresses);
        };

        private Because of = () =>
        {
            results = addressDataManager.FindAddressFromStreetAddress(streetAddress);
        };

        private It should_query_for_an_existing_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<AddressDataModel>(Param.IsAny<ISqlStatement<AddressDataModel>>()));
        };

        private It should_return_the_key_for_the_existing_address = () =>
        {
            results.First().AddressKey.ShouldEqual(existingStreetAddresses.First().AddressKey);
        };
    }
}