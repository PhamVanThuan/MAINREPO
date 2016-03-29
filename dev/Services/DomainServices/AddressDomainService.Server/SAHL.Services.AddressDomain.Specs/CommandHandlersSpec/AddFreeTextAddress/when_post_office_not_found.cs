using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddPostalAddress
{
    public class when_post_office_not_found : WithCoreFakes
    {
        private static AddFreeTextAddressCommandHandler handler;
        private static AddFreeTextAddressCommand command;
        private static IAddressDataManager addressDataManager;
        private static FreeTextAddressModel freeTextAddressModel;
        private static Guid addressGuid;
        private static int addressKey;

        private Establish context = () =>
        {
            addressDataManager = An<IAddressDataManager>();
            handler = new AddFreeTextAddressCommandHandler(addressDataManager, linkedKeyManager);
            freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby Way", "Sydney", "", "", "", "Australia");
            addressGuid = CombGuid.Instance.Generate();
            addressKey = 15;
            command = new AddFreeTextAddressCommand(freeTextAddressModel, addressGuid);
            addressDataManager.WhenToldTo(x => x.SaveAddress(Param.IsAny<AddressDataModel>())).Return(addressKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_the_post_office_key_for_the_country = () =>
        {
            addressDataManager.WasToldTo(x => x.GetPostOfficeKeyForCountry(command.FreeTextAddress.Country));
        };

        private It should_not_add_the_address_ = () =>
        {
            addressDataManager.WasNotToldTo(x => x.SaveAddress(Param.IsAny<AddressDataModel>()));
        };

        private It should_not_link_the_address_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(addressKey, command.AddressId));
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(String.Format("Post Office not found for country : {0}", command.FreeTextAddress.Country));
        };
    }
}