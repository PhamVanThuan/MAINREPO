using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.Services.Extensions;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class ApplyForNewPurchaseCommand : ServiceCommand, ICapitecServiceCommand, IValidation
    {
        public Guid ApplicationID { get; protected set; }
        public NewPurchaseApplication NewPurchaseApplication { get; protected set; }
        
        public ApplyForNewPurchaseCommand(Guid applicationID, NewPurchaseApplication newPurchaseApplication)
        {
            this.ApplicationID = applicationID;
            this.NewPurchaseApplication = newPurchaseApplication;
        }
    }
}