using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
      where T : ApplicationCreationModel
    {
        public override void HandledEvent(IServiceRequestMetadata metadata)
        {
            if (this.ProcessState != null)
            {
                var applicationStateMachine = this.ProcessState as IApplicationStateMachine;
                applicationStateMachine.RecordEventReceived(Guid.Parse(metadata[DomainProcessManagerGlobals.CommandCorrelationKey]));

                var correlationId = Guid.Parse(metadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
                if (applicationStateMachine.HasProcessCompletedWithCriticalPathFullyCaptured())
                {
                    CompleteDomainProcess(correlationId);
                }
            }
        }

        private void PopulateDictionary(IDictionary<string, int> dictionary, string key, int value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        private void CompleteDomainProcess(Guid guid)
        {            
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.CompletionConfirmed, guid);
            base.LogInfoMessage(
                    String.Format("Domain process with ID {0} for application number {1} has completed.",
                    this.DomainProcessId, applicationStateMachine.ApplicationNumber));
            var nonCriticalErrorMessages = applicationStateMachine.SystemMessages.ErrorMessages();
            if (nonCriticalErrorMessages.Any())
            {
                communicationManager.SendNonCriticalErrorsEmail(new SystemMessageCollection(nonCriticalErrorMessages.ToList()), applicationStateMachine.ApplicationNumber);
            }
            OnCompleted(DomainProcessId);            
        }

        private void HandleNonCriticalException(Exception runtimeException, string friendlyErrorMessage, Guid correlationId, IApplicationStateMachine applicationStateMachine)
        {
            base.LogErrorMessage(friendlyErrorMessage);
            var serviceMessages = SystemMessageCollection.Empty();
            serviceMessages.AddMessage(new SystemMessage(runtimeException.ToString(), SystemMessageSeverityEnum.Exception));
            serviceMessages.AddMessage(new SystemMessage(friendlyErrorMessage, SystemMessageSeverityEnum.Error));
            CheckForNonCriticalErrors(applicationStateMachine, correlationId, serviceMessages, ApplicationState.NonCriticalErrorOccured);
        }

        private void HandleCriticalException(Exception runtimeException, string friendlyErrorMessage, Guid correlationId)
        {
            var serviceMessages = SystemMessageCollection.Empty();
            serviceMessages.AddMessage(new SystemMessage(runtimeException.ToString(), SystemMessageSeverityEnum.Exception));
            serviceMessages.AddMessage(new SystemMessage(friendlyErrorMessage, SystemMessageSeverityEnum.Error));
            CheckForCriticalErrors(applicationStateMachine, correlationId, serviceMessages);
        }

        public void CheckForCriticalErrors(IApplicationStateMachine applicationStateMachine, Guid guid, ISystemMessageCollection serviceMessages)
        {
            if (serviceMessages.HasErrors)
            {
                applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.CriticalErrorReported, guid);
                applicationStateMachine.RecordCommandFailed(guid);

                StringBuilder errorMessageBuilder = new StringBuilder();
                serviceMessages.ErrorMessages().ToList().ForEach(x => errorMessageBuilder.Append(x.Message + "\n"));

                var friendlyMessage = String.Format("Critical path failed for application {0} on domain process ID {1}. {2}",
                    applicationStateMachine.ApplicationNumber, this.DomainProcessId, errorMessageBuilder.ToString());
                base.LogErrorMessage(friendlyMessage);

                foreach (var message in serviceMessages.AllMessages)
                {
                    base.LogErrorMessage(message.Message);
#if DEBUG
                    Console.WriteLine(message.Message);
#endif
                }
                applicationDataManager.RollbackCriticalPathApplicationData(applicationStateMachine.ApplicationNumber, applicationStateMachine.EmploymentKeys);
                OnErrorOccurred(DomainProcessId, errorMessageBuilder.ToString());
                throw new Exception(friendlyMessage);
            }
            applicationStateMachine.AggregateMessages(serviceMessages);
        }

        public void CheckForNonCriticalErrors(IApplicationStateMachine applicationStateMachine, Guid guid, ISystemMessageCollection serviceMessages, ApplicationState state)
        {
            if (serviceMessages.HasErrors)
            {
                applicationStateMachine.AdjustStateExpectations(state);
                applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.NonCriticalErrorReported, guid);
                applicationStateMachine.RecordCommandFailed(guid);      
            }
#if DEBUG
            foreach (var message in serviceMessages.AllMessages)
            {
                Console.WriteLine(message.Message);
            }
#endif
            applicationStateMachine.AggregateMessages(serviceMessages);
        }

        public void HandleValidateException(IApplicationStateMachine applicationStateMachine)
        {
            if (!applicationStateMachine.SystemMessages.AllMessages.Any()) 
            {
                OnErrorOccurred(DomainProcessId, "Invalid application received.");
                return; 
            }

            var validationMessageBuilder = new StringBuilder("");
            foreach (var message in applicationStateMachine.SystemMessages.AllMessages)
            {
                validationMessageBuilder.Append(message.Message.ToString());
                validationMessageBuilder.Append("\n");
            }
            OnErrorOccurred(DomainProcessId, validationMessageBuilder.ToString());
            throw new Exception(String.Format("Invalid Application. Reason(s): {0}", validationMessageBuilder.ToString()));
        }

        private void PerformOperationsFollowingEmploymentAddition(IApplicationStateMachine applicationStateMachine)
        {
            if (applicationStateMachine.ClientEmploymentsFullyCaptured())
            {
                AddApplicationHousholdIncome(applicationStateMachine);

                AddApplicationEmploymentType(applicationStateMachine);
            }
        }
    }
}