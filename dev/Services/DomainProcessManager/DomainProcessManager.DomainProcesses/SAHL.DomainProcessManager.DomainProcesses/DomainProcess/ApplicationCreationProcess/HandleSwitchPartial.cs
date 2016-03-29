using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<SwitchApplicationAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(SwitchApplicationAddedEvent switchApplicationAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            ApplicationCreationReturnDataModel returnData = new ApplicationCreationReturnDataModel(switchApplicationAddedEvent.ApplicationNumber);
            this.StartResultData = returnData;

            var addSwitchLinkingGuid = Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
            this.LinkedKeyManager.DeleteLinkedKey(addSwitchLinkingGuid);

            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;
            applicationStateMachine.TriggerBasicApplicationCreated(switchApplicationAddedEvent.Id, switchApplicationAddedEvent.ApplicationNumber);

            foreach (var applicant in this.DataModel.Applicants)
            {
                AddApplicant(applicant);
            }
        }
        public void HandleException(SwitchApplicationAddedEvent switchApplicationAddedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain Process failed after creating application.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}