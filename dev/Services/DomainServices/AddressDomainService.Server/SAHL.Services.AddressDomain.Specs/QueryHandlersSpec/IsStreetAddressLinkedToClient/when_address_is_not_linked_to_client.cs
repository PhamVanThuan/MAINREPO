using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.QueryHandlers;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.QueryHandlersSpec.IsStreetAddressLinkedToClient
{
    class when_address_is_not_linked_to_client : WithFakes
    {
        private static GetClientStreetAddressQuery query;
        private static GetClientStreetAddressQueryHandler handler;
        private static IAddressDataManager addressDataManager;
        private static LegalEntityAddressDataModel legalEntityAddress;
        private static ISystemMessageCollection systemMessageCollection;

        private static StreetAddressModel streetAddressModel;
        private static AddressDataModel[] addresses;

        private static int clientKey;

        private static IEnumerable<int> clientAddressKey;
        private Establish context = () =>
        {
            streetAddressModel = new StreetAddressModel("", "", "", "7", "Maryland Avenue", "Durban North", "Durban", "Kwazulu-Natal", "4051");
            addresses = new AddressDataModel[] { };
            systemMessageCollection = new SystemMessageCollection();
            clientKey = 10001;

            legalEntityAddress = new LegalEntityAddressDataModel(1, 2, 1, DateTime.Now, 1);
            clientAddressKey = new List<int>() { 1 };

            addressDataManager = An<IAddressDataManager>();

            addressDataManager.WhenToldTo(x => x.FindAddressFromStreetAddress(Param.IsAny<StreetAddressModel>())).Return(addresses);

            query = new GetClientStreetAddressQuery(clientKey, streetAddressModel, Core.BusinessModel.Enums.AddressType.Postal);
            handler = new GetClientStreetAddressQueryHandler(addressDataManager);
        };

        private Because of = () =>
        {
            systemMessageCollection = handler.HandleQuery(query);
        };

        private It should_search_for_the_address_on_system = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromStreetAddress(streetAddressModel));
        };

        private It should_not_determine_if_there_is_client_linked_address = () =>
        {
            addressDataManager.WasNotToldTo(x => x.GetExistingActiveClientAddress(clientKey, Param.IsAny<int>(), Core.BusinessModel.Enums.AddressType.Postal));
        };

        private It should__not_contain_an_error_message = () =>
        {
            systemMessageCollection.AllMessages.ShouldBeEmpty();
        };

        private It should_set_the_query_result = () =>
        {
            query.Result.ShouldNotBeNull();
            query.Result.Results.FirstOrDefault().ShouldBeNull();
        };
    }
}
