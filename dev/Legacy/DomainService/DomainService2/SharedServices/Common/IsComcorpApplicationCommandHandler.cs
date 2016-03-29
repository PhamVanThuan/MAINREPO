using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class IsComcorpApplicationCommandHandler : IHandlesDomainServiceCommand<IsComcorpApplicationCommand>
    {
        private IApplicationReadOnlyRepository applicationReadOnlyRepository;

        public IsComcorpApplicationCommandHandler(IApplicationReadOnlyRepository applicationReadOnlyRepository)
        {
            this.applicationReadOnlyRepository = applicationReadOnlyRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsComcorpApplicationCommand command)
        {
            IApplication application = this.applicationReadOnlyRepository.GetApplicationByKey(command.ApplicationKey);
            command.Result = application.IsComcorp();
        }
    }
}