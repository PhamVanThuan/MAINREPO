using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_inserting_comcorpOfferDetails : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static InsertComcorpOfferPropertyDetailsCommand command;
        private static InsertComcorpOfferPropertyDetailsCommandHandler commandHandler;
        private static ComcorpOfferPropertyDetailsDataModel propertyDataModel;

        private Establish context = () =>
            {
                testDataManager = An<ITestDataManager>();
                metadata = An<IServiceRequestMetadata>();
                command = new InsertComcorpOfferPropertyDetailsCommand(propertyDataModel);
                commandHandler = new InsertComcorpOfferPropertyDetailsCommandHandler(testDataManager);
            };

        private Because of = () =>
            {
                messages = commandHandler.HandleCommand(command, metadata);
            };

        private It should_not_return_error_messages = () =>
            {
                messages.HasErrors.ShouldBeFalse();
            };

        private It should_insert_the_comcorp_offer = () =>
            {
                testDataManager.WasToldTo(x => x.InsertComcorpOfferPropertyDetails(propertyDataModel));
            };
    }
}