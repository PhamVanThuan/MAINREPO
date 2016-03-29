using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddStreetAddress
{
    public class when_a_suburb_cannot_be_found_for_an_address : WithCoreFakes
    {
        private static AddStreetAddressCommandHandler handler;
        private static AddStreetAddressCommand command;
        private static IDomainRuleManager<AddressDataModel> domainRuleContext;
        private static IAddressDataManager addressDataManager;
        private static Guid addressGuid;
        private static StreetAddressModel streetAddress;
        private static IEnumerable<SuburbDataModel> suburbs;

        private Establish context = () =>
        {
            suburbs = Enumerable.Empty<SuburbDataModel>();
            addressDataManager = An<IAddressDataManager>();
            serviceRequestMetaData = new ServiceRequestMetadata();

            domainRuleContext = An<IDomainRuleManager<AddressDataModel>>();
            addressGuid = CombGuid.Instance.Generate();
            streetAddress = new StreetAddressModel("", "", "", "12", "Smith Street", "Durban", "Durban", "Kwazulu-Natal", "4001");

            handler = new AddStreetAddressCommandHandler(domainRuleContext, addressDataManager, linkedKeyManager);
            command = new AddStreetAddressCommand(streetAddress, addressGuid);

            addressDataManager.WhenToldTo(x => x.GetSuburbForModelData(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(suburbs);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_execute_rules = () =>
        {
            domainRuleContext.WasNotToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AddressDataModel>()));
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain(
                string.Format("No Suburb could be found for - Suburb: {0}; City: {1}; Province: {2}; PostalCode: {3}", command.StreetAddressModel.Suburb, command.StreetAddressModel.City,
                command.StreetAddressModel.Province, command.StreetAddressModel.PostalCode));
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