using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Linq;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Application;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class UpdateCaptureEndTimeCommandHandler : IServiceCommandHandler<UpdateCaptureEndTimeCommand>
    {
        private IApplicationManager applicationService;
        public UpdateCaptureEndTimeCommandHandler(IApplicationManager applicationService)
        {
            this.applicationService = applicationService;
        }
        public ISystemMessageCollection HandleCommand(UpdateCaptureEndTimeCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            double captureEndTimestamp = double.Parse(command.CaptureEndTimestamp.Split('-').First());
            DateTime captureEndTime = new DateTime(1970, 1, 1).AddMilliseconds(captureEndTimestamp);
            applicationService.UpdateCaptureEndTime(command.ApplicationID, captureEndTime);

            return messages;
        }
    }
}
