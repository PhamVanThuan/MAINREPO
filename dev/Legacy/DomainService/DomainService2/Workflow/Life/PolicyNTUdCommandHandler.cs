namespace DomainService2.Workflow.Life
{
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class PolicyNTUdCommandHandler : IHandlesDomainServiceCommand<PolicyNTUdCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private ILifeRepository LifeRepository;

        public PolicyNTUdCommandHandler(IApplicationRepository applicationRepository, ILifeRepository lifeRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.LifeRepository = lifeRepository;
        }

        public void Handle(IDomainMessageCollection messages, PolicyNTUdCommand command)
        {
            // Get the application
            IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

            // Close the Account & Save the Stage Transition
            LifeRepository.CloseLifeApplication(applicationLife.Account.Key, applicationLife.Key, "NTU'd Application Archived");
        }
    }
}