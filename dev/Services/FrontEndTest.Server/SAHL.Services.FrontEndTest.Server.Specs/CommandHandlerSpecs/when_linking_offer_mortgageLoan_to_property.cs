using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_linking_offer_mortgageLoan_to_property : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static LinkOfferMortgageLoanPropertyCommandHandler commandHandler;
        private static LinkOfferMortgageLoanPropertyCommand command;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            command = new LinkOfferMortgageLoanPropertyCommand(1, 1);
            commandHandler = new LinkOfferMortgageLoanPropertyCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_link_offer_to_property = () =>
        {
            testDataManager.WasToldTo(x => x.LinkOfferMortgageLoanProperty(command.OfferKey, command.PropertyKey));
        };

    }
}
