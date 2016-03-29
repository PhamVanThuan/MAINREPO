using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<EmployerAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(EmployerAddedEvent employerAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var employerLinkedKeyGuid = Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
            this.LinkedKeyManager.DeleteLinkedKey(employerLinkedKeyGuid);

            var employeeIdNumber = serviceRequestMetadata["EmployeeIdNumber"];
            var applicant = DataModel.Applicants.Single(a => a.IDNumber == employeeIdNumber);
            var employment = applicant.Employments.Single(e => e.Employer.EmployerName == employerAddedEvent.EmployerName);
            var clientKey = applicationStateMachine.ClientCollection[applicant.IDNumber];

            AddEmployment(employment, clientKey);
        }

        public void HandleException(EmployerAddedEvent employerAddedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, String.Format("Domain Process failed after adding employer {0}", employerAddedEvent.EmployerName),
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}