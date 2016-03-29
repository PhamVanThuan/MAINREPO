using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when_a_postnet_suite_has_no_suite_number : WithCoreFakes
    {
        private static AddPostalAddressCommandHandler handler;
        private static AddPostalAddressCommand command;
        private static IAddressDataManager addressDataManager;
        private static PostalAddressModel postalAddress;
        private static Guid addressGuid;
        private static IDomainRuleManager<AddressDataModel> domainRuleManager;
        private static int expectedPostOfficeKey;

        private Establish context = () =>
        {
            addressGuid = CombGuid.Instance.Generate();
            addressDataManager = An<IAddressDataManager>();
            postalAddress = new PostalAddressModel("22", null, "Hillcrest", "Gauteng", "Johannesburg", "1011", AddressFormat.PostNetSuite);
            expectedPostOfficeKey = 1;
            addressDataManager
                .WhenToldTo(x => x.GetPostOfficeForModelData(postalAddress.Province, postalAddress.City, postalAddress.PostalCode))
                .Return(new[]
                {
                    new PostOfficeDataModel(expectedPostOfficeKey, "XXX", postalAddress.PostalCode, 1)
                });
            domainRuleManager = An<IDomainRuleManager<AddressDataModel>>();
            handler = new AddPostalAddressCommandHandler(domainRuleManager, addressDataManager, linkedKeyManager);
            command = new AddPostalAddressCommand(postalAddress, addressGuid);

            domainRuleManager
                .WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AddressDataModel>()))
                .Callback<ISystemMessageCollection, AddressDataModel>((y, z) =>
                {
                    y.AddMessage(new SystemMessage("A postnet suite address requires a postnet suite number.", SystemMessageSeverityEnum.Error));
                });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain("A postnet suite address requires a postnet suite number.");
        };

        private It should_not_save_the_address = () =>
        {
            addressDataManager.WasNotToldTo(x => x.SaveAddress(Param.IsAny<AddressDataModel>()));
        };

        private It should_not_link_the_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), command.AddressId));
        };
    }
}
