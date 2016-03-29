namespace DomainService2.Workflow.Life
{
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class GetPolicyTypeKeyForOfferLifeCommandHandler : IHandlesDomainServiceCommand<GetPolicyTypeKeyForOfferLifeCommand>
    {
        private IApplicationRepository ApplicationRepository;

        public GetPolicyTypeKeyForOfferLifeCommandHandler(IApplicationRepository applicationRepository)
        {
            this.ApplicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetPolicyTypeKeyForOfferLifeCommand command)
        {
            // Get the application
            IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

            command.PolicyTypeKeyResult = applicationLife.LifePolicyType.Key;
        }
    }
}