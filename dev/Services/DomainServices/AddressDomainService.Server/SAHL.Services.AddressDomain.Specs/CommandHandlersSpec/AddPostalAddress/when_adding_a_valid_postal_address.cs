using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddPostalAddress
{
    public class when_adding_a_valid_postal_address : WithCoreFakes
    {
        private static AddPostalAddressCommandHandler handler;
        private static AddPostalAddressCommand command;
        private static IDomainRuleManager<AddressDataModel> domainRuleManager;
        private static IAddressDataManager addressDataManager;
        private static PostalAddressModel postalAddress;
        private static Guid addressGuid;

        private Establish context = () =>
        {
            addressDataManager = An<IAddressDataManager>();
            domainRuleManager = Substitute.For<IDomainRuleManager<AddressDataModel>>();
            handler = new AddPostalAddressCommandHandler(domainRuleManager, addressDataManager, linkedKeyManager);
            postalAddress = new PostalAddressModel("12", "", "Hillcrest", "Kwazulu-Natal", "Hillcrest", "3650", AddressFormat.Box);
            addressGuid = CombGuid.Instance.Generate();
            command = new AddPostalAddressCommand(postalAddress, addressGuid);
            addressDataManager
                .WhenToldTo(x => x.GetPostOfficeForModelData(postalAddress.Province, postalAddress.City, postalAddress.PostalCode))
                .Return(new[] { new PostOfficeDataModel(1, "XXX", postalAddress.PostalCode, 1) });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_find_a_post_office_key_for_the_address_provided = () =>
        {
            addressDataManager.WasToldTo(x => x.GetPostOfficeForModelData(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

        private It should_ensure_that_the_address_is_valid = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AddressDataModel>()));
        };

        private It should_add_the_address_ = () =>
        {
            addressDataManager.WasToldTo(x => x.SaveAddress(Param.IsAny<AddressDataModel>()));
        };

        private It should_link_the_address_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), command.AddressId));
        };
    }
}
