namespace DomainService2.Workflow.Origination.FurtherLending
{
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    public class OptOutSuperLoCommandHandler : IHandlesDomainServiceCommand<OptOutSuperLoCommand>
    {
        private IApplicationRepository ApplicationRepository;

        public OptOutSuperLoCommandHandler(IApplicationRepository applicationRepository)
        {
            this.ApplicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, OptOutSuperLoCommand command)
        {
            command.Result = ApplicationRepository.OptOutOfSuperLo(command.ApplicationKey, command.ADUser, (int)CancellationReasons.CancelSuperLo);
        }
    }
}