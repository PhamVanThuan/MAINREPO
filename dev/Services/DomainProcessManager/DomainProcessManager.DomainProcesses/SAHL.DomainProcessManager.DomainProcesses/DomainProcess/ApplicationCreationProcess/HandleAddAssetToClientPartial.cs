using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;
using SAHL.Core.SystemMessages;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
       , IDomainProcessEvent<OtherAssetAddedToClientEvent>
       , IDomainProcessEvent<FixedLongTermInvestmentLiabilityAddedToClientEvent>
       , IDomainProcessEvent<FixedPropertyAssetAddedToClientEvent>
       , IDomainProcessEvent<LifeAssuranceAssetAddedToClientEvent>
       , IDomainProcessEvent<InvestmentAssetAddedToClientEvent>
       , IDomainProcessEvent<LiabilitySuretyAddedToClientEvent>
       , IDomainProcessEvent<LiabilityLoanAddedToClientEvent>
       where T : ApplicationCreationModel
    {
        public void Handle(OtherAssetAddedToClientEvent otherAssetAddedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            FireAssetAddedToClientConfirmedTrigger(otherAssetAddedToClientEvent.Id);
        }

        public void Handle(FixedLongTermInvestmentLiabilityAddedToClientEvent fixedLongTermInvestmentLiabilityAddedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            FireAssetAddedToClientConfirmedTrigger(fixedLongTermInvestmentLiabilityAddedToClientEvent.Id);
        }

        public void Handle(FixedPropertyAssetAddedToClientEvent fixedPropertyAssetAddedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            FireAssetAddedToClientConfirmedTrigger(fixedPropertyAssetAddedToClientEvent.Id);
        }

        public void Handle(LifeAssuranceAssetAddedToClientEvent lifeAssuranceAssetAddedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            FireAssetAddedToClientConfirmedTrigger(lifeAssuranceAssetAddedToClientEvent.Id);
        }

        public void Handle(InvestmentAssetAddedToClientEvent investmentAssetAddedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            FireAssetAddedToClientConfirmedTrigger(investmentAssetAddedToClientEvent.Id);
        }

        public void Handle(LiabilityLoanAddedToClientEvent liabilityLoanAddedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            FireAssetAddedToClientConfirmedTrigger(liabilityLoanAddedToClientEvent.Id);
        }

        public void Handle(LiabilitySuretyAddedToClientEvent liabilitySuretyAddedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            FireAssetAddedToClientConfirmedTrigger(liabilitySuretyAddedToClientEvent.Id);
        }

        public void FireAssetAddedToClientConfirmedTrigger(Guid id)
        {
            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, id);
        }
    }
}