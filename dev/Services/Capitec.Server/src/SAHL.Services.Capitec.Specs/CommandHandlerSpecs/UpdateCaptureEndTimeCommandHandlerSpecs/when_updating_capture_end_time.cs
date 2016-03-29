using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Application;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.UpdateCaptureEndTimeCommandHandlerSpecs
{
    public class when_updating_capture_end_time : WithFakes
    {
        private static IApplicationManager applicationService;
        private static UpdateCaptureEndTimeCommandHandler commandHandler;
        private static UpdateCaptureEndTimeCommand command;
        private static Guid applicationID;
        private static string unixTimestamp;
        private static DateTime expectedDate;
        private static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            applicationService = An<IApplicationManager>();
            commandHandler = new UpdateCaptureEndTimeCommandHandler(applicationService);

            unixTimestamp = "1405085710574-0200";
            applicationID = Guid.NewGuid();
            command = new UpdateCaptureEndTimeCommand(applicationID, unixTimestamp);

            expectedDate = new DateTime(2014, 7, 11, 13, 35, 10, 574);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command,metadata);
        };

        private It should_update_the_capture_end_time_with_the_date = () =>
        {
            applicationService.WasToldTo(x => x.UpdateCaptureEndTime(applicationID, Param<DateTime>.Matches(y => y == expectedDate)));
        };

        private It should_return_no_errors = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}