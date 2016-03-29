using SAHL.Core.DomainProcess;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        protected void CreateWorkflowCase(Guid triggeringEventGuid)
        {
            Guid requestInstanceCorrelationId = combGuidGenerator.Generate();
            var domainProcessServiceMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, requestInstanceCorrelationId);
            try
            {
                if (!applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.X2CaseCreated))
                {
                    var x2Messages = x2WorkflowManager.CreateWorkflowCase(applicationStateMachine.ApplicationNumber, domainProcessServiceMetadata);
                    applicationStateMachine.SystemMessages.Aggregate(x2Messages);
                    if (!x2Messages.HasErrors)
                    {
                        applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, requestInstanceCorrelationId);
                    }
                    else
                    {
                        base.LogInfoMessage("Create X2 Case returned error message!");
                        applicationStateMachine.AggregateMessages(x2Messages);
                        communicationManager.SendX2CaseCreationFailedSupportEmail(x2Messages, this.DomainProcessId, applicationStateMachine.ApplicationNumber);
                    }
                }
            }
            catch (Exception runtimeException)
            {
                string friendlyErrorMessage = "Failed to create X2 Workflow Case.";
                base.LogErrorMessage(friendlyErrorMessage);
                var serviceMessages = SystemMessageCollection.Empty();
                serviceMessages.AddMessage(new SystemMessage(runtimeException.ToString(), SystemMessageSeverityEnum.Exception));
                serviceMessages.AddMessage(new SystemMessage(friendlyErrorMessage, SystemMessageSeverityEnum.Error));
                applicationStateMachine.AggregateMessages(serviceMessages);
                communicationManager.SendX2CaseCreationFailedSupportEmail(serviceMessages, this.DomainProcessId, applicationStateMachine.ApplicationNumber);
            }
        }
    }
}