using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddPostalAddress
{
    public class when_a_post_office_cannot_be_found_for_an_address : WithCoreFakes
    {
        private static AddPostalAddressCommandHandler handler;
        private static AddPostalAddressCommand command;
        private static IAddressDataManager addressDataManager;
        private static PostalAddressModel postalAddress;
        private static Guid addressGuid;
        private static IEnumerable<PostOfficeDataModel> postOffices;
        private static IDomainRuleManager<AddressDataModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<AddressDataModel>>();
            postOffices = Enumerable.Empty<PostOfficeDataModel>();
            addressGuid = CombGuid.Instance.Generate();
            addressDataManager = An<IAddressDataManager>();
            postalAddress = new PostalAddressModel("22", null, "Hillcrest", "Gauteng", "Johannesburg", "9999", AddressFormat.PostNetSuite);
            handler = new AddPostalAddressCommandHandler(domainRuleManager, addressDataManager, linkedKeyManager);
            command = new AddPostalAddressCommand(postalAddress, addressGuid);
            addressDataManager.WhenToldTo(x => x.GetPostOfficeForModelData(postalAddress.Province, postalAddress.City, postalAddress.PostalCode))
                .Return(postOffices);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages()
                .First()
                .Message.ShouldContain(string.Format("No Post Office could be found for - City: {0}; Province: {1}; PostalCode: {2}",
                    command.PostalAddressModel.City,
                    command.PostalAddressModel.Province,
                    command.PostalAddressModel.PostalCode));
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
