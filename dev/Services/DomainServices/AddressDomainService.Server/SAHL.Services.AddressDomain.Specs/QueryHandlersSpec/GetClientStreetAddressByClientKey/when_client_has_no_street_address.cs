using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.QueryHandlers;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.QueryHandlersSpec.GetClientStreetAddressByClientKey
{
    public class when_client_has_street_address : WithFakes
    {
        private static GetClientStreetAddressByClientKeyQuery query;
        private static GetClientStreetAddressByClientKeyQueryHandler handler;
        private static IAddressDataManager addressDataManager;
        private static ISystemMessageCollection systemMessageCollection;
        private static AddressDataModel[] addresses;

        private static int clientKey;

        private Establish context = () =>
        {
            addresses = new AddressDataModel[] { };
            systemMessageCollection = new SystemMessageCollection();
            clientKey = 10001;

            addressDataManager = An<IAddressDataManager>();
            addressDataManager.WhenToldTo(x => x.GetExistingActiveClientStreetAddressByClientKey(clientKey))
                                .Return(addresses);
            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(addresses);

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

        private It should_set_the_query_result = () =>
        {
            query.Result.Results.Count().ShouldEqual(addresses.Count());
        };
    }
}