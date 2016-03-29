using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.Services.Extensions;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class UpdateCaptureEndTimeCommand : ServiceCommand, ICapitecServiceCommand
    {
        public Guid ApplicationID { get; protected set; }

        public string CaptureEndTimestamp { get; protected set; }

        public UpdateCaptureEndTimeCommand(Guid applicationID, string captureEndTimestamp)
        {
            this.ApplicationID = applicationID;
            this.CaptureEndTimestamp = captureEndTimestamp;
        }
    }
}