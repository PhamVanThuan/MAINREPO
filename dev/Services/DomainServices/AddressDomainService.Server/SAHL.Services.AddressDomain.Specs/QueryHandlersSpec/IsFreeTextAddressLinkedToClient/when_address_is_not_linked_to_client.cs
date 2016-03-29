using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
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

namespace SAHL.Services.Interfaces.AddressDomain.Specs.QueryHandlersSpec.IsFreeTextAddressLinkedToClient
{
    class when_address_is_not_linked_to_client : WithFakes
    {
        private static GetClientFreeTextAddressQuery query;
        private static GetClientFreeTextAddressQueryHandler handler;
        private static IAddressDataManager addressDataManager;
        private static LegalEntityAddressDataModel legalEntityAddress;
        private static ISystemMessageCollection systemMessageCollection;

        private static FreeTextAddressModel freeTextAddressModel;
        private static AddressDataModel[] addresses;

        private static int addressKey, clientKey;

        private static IEnumerable<int> clientAddressKey;
        private Establish context = () =>
        {
            addressKey = 0;
            freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby Way", "Sydney", "", "", "", "Australia");
            addresses = new AddressDataModel[] { new AddressDataModel(addressKey, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                null, null, "42 Wallaby Way", "Sydney", "", "", "") };
            addresses = new AddressDataModel[] { };
            systemMessageCollection = new SystemMessageCollection();
            clientKey = 10001;

            legalEntityAddress = new LegalEntityAddressDataModel(1, 2, 1, DateTime.Now, 1);
            clientAddressKey = new List<int>() { 1 };

            addressDataManager = An<IAddressDataManager>();
            addressDataManager.WhenToldTo(x => x.FindAddressFromFreeTextAddress(freeTextAddressModel)).Return(addresses);

            query = new GetClientFreeTextAddressQuery(clientKey, freeTextAddressModel, Core.BusinessModel.Enums.AddressType.Postal);
            handler = new GetClientFreeTextAddressQueryHandler(addressDataManager);
        };

        private Because of = () =>
        {
            systemMessageCollection = handler.HandleQuery(query);
        };

        private It should_search_for_the_address = () =>
        {
            addressDataManager.WasToldTo(x => x.FindAddressFromFreeTextAddress(freeTextAddressModel));
        };

        private It should_not_determine_if_there_is_a_client_linked_to_address = () =>
        {
            addressDataManager.WasNotToldTo(x => x.GetExistingActiveClientAddress(clientKey, Param.IsAny<int>(), Core.BusinessModel.Enums.AddressType.Postal));
        };

        private It should__not_contain_an_error_message = () =>
        {
            systemMessageCollection.AllMessages.ShouldBeEmpty();
        };

        private It should_not_set_the_query_result = () =>
        {
            query.Result.ShouldNotBeNull();
            query.Result.Results.FirstOrDefault().ShouldBeNull();
        };
    }
}
