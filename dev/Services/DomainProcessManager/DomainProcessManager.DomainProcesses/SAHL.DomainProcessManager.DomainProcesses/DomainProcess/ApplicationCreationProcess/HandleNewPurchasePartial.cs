using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<NewPurchaseApplicationAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(NewPurchaseApplicationAddedEvent applicationCreatedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            ApplicationCreationReturnDataModel returnData = new ApplicationCreationReturnDataModel(applicationCreatedEvent.ApplicationNumber);
            this.StartResultData = returnData;

            var addNewPurchaseLinkingGuid = Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
            this.LinkedKeyManager.DeleteLinkedKey(addNewPurchaseLinkingGuid);

            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;
            applicationStateMachine.TriggerBasicApplicationCreated(applicationCreatedEvent.Id, applicationCreatedEvent.ApplicationNumber);

            foreach (var applicant in this.DataModel.Applicants)
            {
                AddApplicant(applicant);
            }
        }

        public void HandleException(NewPurchaseApplicationAddedEvent applicationCreatedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain Process failed after creating application.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }

    }
}