using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class SetOfferEndDateCommandHandler : IHandlesDomainServiceCommand<SetOfferEndDateCommand>
    {
        private IApplicationRepository applicationRepository;

        public SetOfferEndDateCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, SetOfferEndDateCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            application.ApplicationEndDate = DateTime.Now;
            applicationRepository.SaveApplication(application);
        }
    }
}