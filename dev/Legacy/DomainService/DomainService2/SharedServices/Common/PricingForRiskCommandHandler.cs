using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class PricingForRiskCommandHandler : IHandlesDomainServiceCommand<PricingForRiskCommand>
    {
        IApplicationRepository applicationRepository;

        public PricingForRiskCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, PricingForRiskCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            application.PricingForRisk();
            applicationRepository.SaveApplication(application);
        }
    }
}