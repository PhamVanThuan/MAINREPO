using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.PropertyDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<ComcorpOfferPropertyDetailsAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(ComcorpOfferPropertyDetailsAddedEvent comcorpOfferPropertyDetailsAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
        }
    }
}