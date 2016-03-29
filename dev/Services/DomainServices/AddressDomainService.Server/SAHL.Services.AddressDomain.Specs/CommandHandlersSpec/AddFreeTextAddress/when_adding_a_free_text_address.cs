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

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddPostalAddress
{
    public class when_adding_a_free_text_address : WithCoreFakes
    {
        private static AddFreeTextAddressCommandHandler handler;
        private static AddFreeTextAddressCommand command;
        private static IAddressDataManager addressDataManager;
        private static FreeTextAddressModel freeTextAddressModel;
        private static Guid addressGuid;
        private static int addressKey, postOfficeKey;

        private Establish context = () =>
        {
            addressDataManager = An<IAddressDataManager>();
            handler = new AddFreeTextAddressCommandHandler(addressDataManager, linkedKeyManager);
            freeTextAddressModel = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby Way", "Sydney", "", "", "", "Australia");
            addressGuid = CombGuid.Instance.Generate();
            addressKey = 15;
            postOfficeKey = 32;
            command = new AddFreeTextAddressCommand(freeTextAddressModel, addressGuid);
            addressDataManager.WhenToldTo(x => x.SaveAddress(Param.IsAny<AddressDataModel>())).Return(addressKey);
            addressDataManager.WhenToldTo(x => x.GetPostOfficeKeyForCountry(command.FreeTextAddress.Country)).Return(postOfficeKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_the_post_office_key_for_the_country = () =>
        {
            addressDataManager.WasToldTo(x => x.GetPostOfficeKeyForCountry(command.FreeTextAddress.Country));
        };

        private It should_add_the_address_ = () =>
        {
            addressDataManager.WasToldTo(x => x.SaveAddress(Param<AddressDataModel>.Matches(m =>
                m.FreeText1 == freeTextAddressModel.FreeText1 &&
                m.FreeText2 == freeTextAddressModel.FreeText2 &&
                m.FreeText3 == freeTextAddressModel.FreeText3 &&
                m.FreeText4 == freeTextAddressModel.FreeText4 &&
                m.FreeText5 == freeTextAddressModel.FreeText5 &&
                m.PostOfficeKey == postOfficeKey &&
                m.AddressFormatKey == (int)freeTextAddressModel.AddressFormat
                )));
        };

        private It should_link_the_address_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(addressKey, command.AddressId));
        };
    }
}