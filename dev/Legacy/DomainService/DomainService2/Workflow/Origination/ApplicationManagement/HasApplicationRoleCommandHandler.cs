using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class HasApplicationRoleCommandHandler : IHandlesDomainServiceCommand<HasApplicationRoleCommand>
    {
        private IApplicationRepository applicationRepository;

        public HasApplicationRoleCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, HasApplicationRoleCommand command)
        {
            var applicationRole = applicationRepository.GetActiveApplicationRoleForTypeAndKey(command.ApplicationKey, command.ApplicationRoleTypeKey);
            command.Result = applicationRole != null;
        }
    }
}