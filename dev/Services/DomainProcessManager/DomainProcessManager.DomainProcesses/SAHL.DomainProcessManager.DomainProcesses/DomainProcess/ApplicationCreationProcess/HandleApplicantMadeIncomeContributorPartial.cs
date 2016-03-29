using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<ApplicantMadeIncomeContributorEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(ApplicantMadeIncomeContributorEvent applicantMadeIncomeContributorEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
        }
    }
}