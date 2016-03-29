namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class ReadyToCallbackCommandHandler : IHandlesDomainServiceCommand<ReadyToCallbackCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private ILifeRepository LifeRepository;

        public ReadyToCallbackCommandHandler(IApplicationRepository applicationRepository, ILifeRepository lifeRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.LifeRepository = lifeRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReadyToCallbackCommand command)
        {
            // only send the email if the callback was completed.
            if (ApplicationRepository.CompleteCallback(command.ApplicationKey, DateTime.Now))
            {
                // Get the application
                IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

                // Sends an internal email with the details of the case appearing on the Ready to Callback worklist
                if (applicationLife.Consultant != null && applicationLife.Consultant.LegalEntity != null && !string.IsNullOrEmpty(applicationLife.Consultant.LegalEntity.EmailAddress))
                    LifeRepository.LifeApplicationReadyToCallback_SendInternalEmail(applicationLife.Account.Key, applicationLife.Key, command.InstanceID, applicationLife.Consultant.LegalEntity.EmailAddress);
            }
        }
    }
}