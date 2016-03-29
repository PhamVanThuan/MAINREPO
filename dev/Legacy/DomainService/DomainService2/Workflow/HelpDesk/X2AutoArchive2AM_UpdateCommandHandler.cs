using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.HelpDesk
{
    public class X2AutoArchive2AM_UpdateCommandHandler : IHandlesDomainServiceCommand<X2AutoArchive2AM_UpdateCommand>
    {
        private IHelpDeskRepository helpDeskRepository;

        public X2AutoArchive2AM_UpdateCommandHandler(IHelpDeskRepository helpDeskRepository)
        {
            this.helpDeskRepository = helpDeskRepository;
        }

        public void Handle(IDomainMessageCollection messages, X2AutoArchive2AM_UpdateCommand command)
        {
            command.Result = helpDeskRepository.X2AutoArchive2AM_Update(command.HelpDeskQueryKey);
        }
    }
}