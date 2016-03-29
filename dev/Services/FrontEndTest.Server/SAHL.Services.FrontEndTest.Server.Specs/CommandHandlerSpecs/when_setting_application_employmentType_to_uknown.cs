using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_setting_application_employmentType_to_uknown : WithFakes
    {
        private static ITestDataManager testDatamanager;
        private static SetApplicationEmploymentTypeToUnknownCommand command;
        private static SetApplicationEmploymentTypeToUnknownCommandHandler commandhandler;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metaData;
        private static int applicationKey;

        private Establish context = () =>
        {
            testDatamanager = An<ITestDataManager>();
            messages = An<ISystemMessageCollection>();
            metaData = An<IServiceRequestMetadata>();
            applicationKey = 123456789;
            command = new SetApplicationEmploymentTypeToUnknownCommand(applicationKey);
            commandhandler = new SetApplicationEmploymentTypeToUnknownCommandHandler(testDatamanager);
        };

        private Because of = () =>
        {
            messages = commandhandler.HandleCommand(command, metaData);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_set_the_applicationSPV_to_0 = () =>
        {
            testDatamanager.WasToldTo(x => x.UpdateApplicationEmploymentType(command.ApplicationNumber, EmploymentType.Unknown));
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}