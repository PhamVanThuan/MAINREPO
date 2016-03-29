using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CreateInternetApplicationCommandHandler : IHandlesDomainServiceCommand<CreateInternetApplicationCommand>
    {
        private IApplicationRepository _applicationRepository;

        public CreateInternetApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, CreateInternetApplicationCommand command)
        {
            ILeadInputInformation applicationInputData = _applicationRepository.DeserializeNetLeadXML(command.ApplicationData);
            command.Result = _applicationRepository.GenerateApplicationFromWeb(command.ApplicationKey, applicationInputData);
        }
    }
}
