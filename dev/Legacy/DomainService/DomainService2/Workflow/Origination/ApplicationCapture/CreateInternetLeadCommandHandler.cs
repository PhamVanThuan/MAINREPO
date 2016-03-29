using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CreateInternetLeadCommandHandler : IHandlesDomainServiceCommand<CreateInternetLeadCommand>
    {
        IApplicationRepository _applicationRepository;

        public CreateInternetLeadCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, CreateInternetLeadCommand command)
        {
            ILeadInputInformation leadInputInfo = _applicationRepository.DeserializeNetLeadXML(command.LeadData);
            command.Result = _applicationRepository.GenerateLeadFromWeb(command.ApplicationKey, leadInputInfo);
        }
    }
}
