using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class ActivatePendingDomiciliumAddressCommandHandler : IHandlesDomainServiceCommand<ActivatePendingDomiciliumAddressCommand>
    {
        IApplicationRepository applicationRepository;

        public ActivatePendingDomiciliumAddressCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ActivatePendingDomiciliumAddressCommand command)
        {
            applicationRepository.ActivatePendingDomiciliumAddress(command.ApplicationKey);
        }
    }
}
