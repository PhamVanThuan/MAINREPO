using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_setting_applicationSPV_to_Zero : WithFakes
    {
        private static ITestDataManager testDatamanager;
        private static SetApplicationSPVToZeroCommand command;
        private static SetApplicationSPVToZeroCommandHandler commandhandler;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metaData;
        private static int applicationKey;

        private Establish context = () =>
        {
            testDatamanager = An<ITestDataManager>();
            messages = An<ISystemMessageCollection>();
            metaData = An<IServiceRequestMetadata>();
            applicationKey = 123456789;
            command = new SetApplicationSPVToZeroCommand(applicationKey);
            commandhandler = new SetApplicationSPVToZeroCommandHandler(testDatamanager);
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
            testDatamanager.WasToldTo(x => x.SetOfferInformationSPV(command.ApplicationInformationKey, 0));
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}