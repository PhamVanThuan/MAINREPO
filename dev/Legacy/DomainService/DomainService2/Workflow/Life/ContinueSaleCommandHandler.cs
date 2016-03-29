namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class ContinueSaleCommandHandler : IHandlesDomainServiceCommand<ContinueSaleCommand>
    {
        private IApplicationRepository ApplicationRepository;

        public ContinueSaleCommandHandler(IApplicationRepository applicationRepository)
        {
            this.ApplicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, ContinueSaleCommand command)
        {
            command.Result = ApplicationRepository.CompleteCallback(command.ApplicationKey, DateTime.Now);
        }
    }
}