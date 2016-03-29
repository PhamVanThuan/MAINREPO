using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        public void AddApplicationMailingAddress()
        {
            var applicationNumber = applicationStateMachine.ApplicationNumber;
            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            try
            {
                var address = this.DataModel.ApplicationMailingAddress;
                if (address != null)
                {
                    if (applicationStateMachine.MailingClientAddressKey.HasValue)
                    {
                        var clientAddressKey = applicationStateMachine.MailingClientAddressKey.Value;

                        if (clientAddressKey > 0)
                        {
                            var applicationMailingAddressModel =
                            new Services.Interfaces.ApplicationDomain.Models.ApplicationMailingAddressModel(applicationNumber, clientAddressKey, address.CorrespondenceLanguage,
                                address.OnlineStatementFormat, address.CorrespondenceMedium, address.ClientToUseForEmailCorrespondence, address.OnlineStatementRequired);

                            var addApplicationMailingAddressCommand = new AddApplicationMailingAddressCommand(applicationMailingAddressModel);

                            var serviceMessages = applicationDomainService.PerformCommand(addApplicationMailingAddressCommand, serviceRequestMetadata);
                            CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.ApplicationMailingAddressCaptured);
                        }
                    }
                }
            }
            catch (Exception runtimeException)
            {
                string friendlyErrorMessage = String.Format("Application Mailing Address could not be saved for application {0}.", applicationNumber);
                HandleNonCriticalException(runtimeException, friendlyErrorMessage, serviceRequestMetadata.CommandCorrelationId, applicationStateMachine);
            }
        }
    }
}