using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [CSJsonifierIgnore]
    public class CreateSAHomeLoansSwitchApplicationCommand : ServiceCommand, ICapitecServiceCommand
    {
        public CreateSAHomeLoansSwitchApplicationCommand(int applicationNumber, ISystemMessageCollection systemMessageCollection, Enumerations.ApplicationStatusEnums applicationType, SwitchLoanApplication switchLoanApplication, Guid applicationId)
        {
            this.ApplicationId = applicationId;
            this.ApplicationType = applicationType;
            this.SwitchLoanApplication = switchLoanApplication;
            this.ApplicationNumber = applicationNumber;
            this.SystemMessageCollection = systemMessageCollection;
        }

        public Guid ApplicationId { get; protected set; }

        public int ApplicationNumber { get; protected set; }

        public Enumerations.ApplicationStatusEnums ApplicationType { get; protected set; }

        public SwitchLoanApplication SwitchLoanApplication { get; protected set; }

        public ISystemMessageCollection SystemMessageCollection { get; protected set; }
    }
}