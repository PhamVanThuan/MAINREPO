using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.AddressDomain.CommandHandlers.Internal;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.AddressDomain.Specs.CommandHandlersSpec.AddStreetAddress
{
    public class when_adding_a_valid_street_address : WithCoreFakes
    {
        private static AddStreetAddressCommandHandler handler;
        private static IDomainRuleManager<AddressDataModel> domainRuleManager;
        private static AddStreetAddressCommand addStreetAddressCommand;
        private static StreetAddressModel streetAddress;
        private static Guid addressGuid;
        private static IAddressDataManager addressDataManager;
        private static IEnumerable<SuburbDataModel> suburbs;

        private Establish context = () =>
        {
            suburbs = new SuburbDataModel[] { new SuburbDataModel("test", 2, "test") };
            addressDataManager = An<IAddressDataManager>();
            addressDataManager.WhenToldTo(x => x.GetSuburbForModelData(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(suburbs);
            addressGuid = CombGuid.Instance.Generate();
            domainRuleManager = An<IDomainRuleManager<AddressDataModel>>();
            handler = new AddStreetAddressCommandHandler(domainRuleManager, addressDataManager, linkedKeyManager);
            streetAddress = new StreetAddressModel("", "", "", "12", "Smith Street", "Durban", "Durban", "Kwazulu-Natal", "4001");
            addStreetAddressCommand = new AddStreetAddressCommand(streetAddress, addressGuid);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(addStreetAddressCommand, serviceRequestMetaData);
        };

        private It should_find_a_suburb_for_the_address_provided = () =>
        {
            addressDataManager.WasToldTo(x => x.GetSuburbForModelData(streetAddress.Suburb, streetAddress.City, streetAddress.PostalCode, streetAddress.Province));
        };

        private It should_have_street_address_validation_domain_rule = () =>
        {
            domainRuleManager.WasToldTo(r => r.RegisterRule(Param.IsAny<StreetAddressRequiresAValidSuburbRule>()));
        };

        private It should_run_the_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AddressDataModel>()));
        };

        private It adds_the_address = () =>
        {
            addressDataManager.WasToldTo(x => x.SaveAddress(Param.IsAny<AddressDataModel>()));
        };

        private It should_map_the_address_to_the_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), addStreetAddressCommand.AddressId));
        };
    }
}