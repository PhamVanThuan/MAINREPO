using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using SAHL.Services.Interfaces.PropertyDomain.Models;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        public void AddComCorpPropertyDetails()
        {
            var commandCorrelationId = combGuidGenerator.Generate();
            try
            {
                DomainModelMapper mapper = new DomainModelMapper();
                mapper.CreateMap<ComcorpApplicationPropertyDetailsModel, ComcorpOfferPropertyDetailsModel>();
                var comcorpPropertyDetailsModel = mapper.Map(this.DataModel.ComcorpApplicationPropertyDetail);

                var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, commandCorrelationId);

                var command = new AddComcorpOfferPropertyDetailsCommand(applicationStateMachine.ApplicationNumber, comcorpPropertyDetailsModel);
                var serviceMessages = PropertyDomainService.PerformCommand(command, serviceRequestMetadata);
                CheckForNonCriticalErrors(applicationStateMachine, commandCorrelationId, serviceMessages, ApplicationState.NonCriticalErrorOccured);
            }
            catch (Exception runtimeException)
            {
                var friendlyErrorMessage = String.Format("Comcorp Property Details could not be saved.");
                HandleNonCriticalException(runtimeException, friendlyErrorMessage, commandCorrelationId, applicationStateMachine);
            }
        }
    }
}