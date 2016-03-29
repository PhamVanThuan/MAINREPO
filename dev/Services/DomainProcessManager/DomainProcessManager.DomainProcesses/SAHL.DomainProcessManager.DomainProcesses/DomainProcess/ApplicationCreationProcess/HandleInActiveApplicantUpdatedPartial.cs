using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<InactiveNaturalPersonClientUpdatedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(InactiveNaturalPersonClientUpdatedEvent existingApplicantUpdatedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var applicationRoleLinkingGuid = Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
            this.LinkedKeyManager.DeleteLinkedKey(applicationRoleLinkingGuid);
        }
    }
}