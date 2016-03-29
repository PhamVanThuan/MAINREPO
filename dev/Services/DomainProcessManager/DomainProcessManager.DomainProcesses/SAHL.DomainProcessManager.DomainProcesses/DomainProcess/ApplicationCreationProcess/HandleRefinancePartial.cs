using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<RefinanceApplicationAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(RefinanceApplicationAddedEvent refinanceApplicationAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            ApplicationCreationReturnDataModel returnData = new ApplicationCreationReturnDataModel(refinanceApplicationAddedEvent.ApplicationNumber);
            this.StartResultData = returnData;

            var addSwitchLinkingGuid = Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
            this.LinkedKeyManager.DeleteLinkedKey(addSwitchLinkingGuid);

            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;
            applicationStateMachine.TriggerBasicApplicationCreated(refinanceApplicationAddedEvent.Id, refinanceApplicationAddedEvent.ApplicationNumber);

            foreach (var applicant in this.DataModel.Applicants)
            {
                AddApplicant(applicant);
            }
        }
        public void HandleException(RefinanceApplicationAddedEvent refinanceApplicationAddedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain Process failed after creating application.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}