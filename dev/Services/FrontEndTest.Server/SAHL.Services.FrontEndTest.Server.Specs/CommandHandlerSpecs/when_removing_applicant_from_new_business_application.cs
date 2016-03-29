using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_applicant_from_new_business_application : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static RemoveApplicantFromActiveNewBusinessApplicantsCommand command;
        private static RemoveApplicantFromActiveNewBusinessApplicantsCommandHandler commandHandler;
        private static int offerRoleKey;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            offerRoleKey = 123;
            command = new RemoveApplicantFromActiveNewBusinessApplicantsCommand(offerRoleKey);
            commandHandler = new RemoveApplicantFromActiveNewBusinessApplicantsCommandHandler(testDataManager);
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

        private It should_remove_the_applicant_from_the_new_business_application = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveActiveNewBusinessApplicantRecord(offerRoleKey));
        };
    }
}