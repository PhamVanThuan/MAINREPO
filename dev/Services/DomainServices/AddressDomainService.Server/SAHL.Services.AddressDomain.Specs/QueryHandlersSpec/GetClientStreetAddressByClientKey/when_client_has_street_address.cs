using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.QueryHandlers;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.QueryHandlersSpec.GetClientStreetAddressByClientKey
{
    class when_client_has_no_street_address : WithFakes
    {
        private static GetClientStreetAddressByClientKeyQuery query;
        private static GetClientStreetAddressByClientKeyQueryHandler handler;
        private static IAddressDataManager addressDataManager;
        private static ISystemMessageCollection systemMessageCollection;

        private static StreetAddressModel streetAddressModel;
        private static AddressDataModel[] addresses;

        private static int addressKey, clientKey;

        private static IEnumerable<int> clientAddressKey;
        private Establish context = () =>
        {
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            addresses = new AddressDataModel[] { new AddressDataModel(1234, 1, null, null, null, null, "1", "Blue Road", 1234, null, 
                "South Africa", "Gauteng", "Gauteng", "Sandton", "1234", "test", null, null,
                null, null,null, null, null)};
            systemMessageCollection = new SystemMessageCollection();
            addressKey = addresses.First().AddressKey;
            clientKey = 10001;

            clientAddressKey = new List<int>() { 1 };

            addressDataManager = An<IAddressDataManager>();
            addressDataManager.WhenToldTo(x => x.GetExistingActiveClientStreetAddressByClientKey(clientKey))
                                .Return(addresses);
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(streetAddressModel)).Return(addresses);

            query = new GetClientStreetAddressByClientKeyQuery(clientKey);
            handler = new GetClientStreetAddressByClientKeyQueryHandler(addressDataManager);
        };

        private Because of = () =>
        {
            systemMessageCollection = handler.HandleQuery(query);
        };

        private It should_search_for_the_address = () =>
        {
            addressDataManager.WasToldTo(x => x.GetExistingActiveClientStreetAddressByClientKey(clientKey));
        };

        private It should_not_contain_an_error_message = () =>
        {
            systemMessageCollection.AllMessages.ShouldBeEmpty();
        };

        private It should_not_contain_any_address_in_the_query_result = () =>
        {
            query.Result.Results.Count().ShouldEqual(addresses.Count());
        };
    }
}
