using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Interfaces.Capitec.Common;
using System;

namespace SAHL.Services.Capitec.Managers.CapitecApplication
{
    public interface ICapitecApplicationManager
    {
        NewPurchaseApplication CreateCapitecApplicationFromNewPurchaseApplication(int applicationNumber, Enumerations.ApplicationStatusEnums applicationStatusEnum, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseApplication capitecNewPurchaseApplication, ISystemMessageCollection messages, Guid ApplicationId);

        SwitchLoanApplication CreateCapitecApplicationFromSwitchLoanApplication(int applicationNumber, Enumerations.ApplicationStatusEnums applicationStatusEnum, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication capitecSwitchLoanApplication, ISystemMessageCollection messages, Guid ApplicationId);
    }
}