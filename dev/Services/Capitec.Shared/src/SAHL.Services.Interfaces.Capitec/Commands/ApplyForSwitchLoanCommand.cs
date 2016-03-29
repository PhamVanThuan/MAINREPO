using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.Services.Extensions;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class ApplyForSwitchLoanCommand : ServiceCommand, ICapitecServiceCommand, IValidation
    {
        public Guid ApplicationID { get; protected set; }

        public SwitchLoanApplication SwitchLoanApplication { get; protected set; }
  
        public ApplyForSwitchLoanCommand(Guid applicationID, SwitchLoanApplication switchLoanApplication)
        {
            this.SwitchLoanApplication = switchLoanApplication;
            this.ApplicationID = applicationID;
        }
    }
}