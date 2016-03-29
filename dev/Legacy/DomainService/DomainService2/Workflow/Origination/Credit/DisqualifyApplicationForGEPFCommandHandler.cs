using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.Credit
{
    public class DisqualifyApplicationForGEPFCommandHandler : IHandlesDomainServiceCommand<DisqualifyApplicationForGEPFCommand>
    {
        private IApplicationRepository applicationRepository;

        public DisqualifyApplicationForGEPFCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, DisqualifyApplicationForGEPFCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application != null)
            {
                applicationRepository.AddApplicationAttributeIfNotExists(application, SAHL.Common.Globals.OfferAttributeTypes.DisqualifiedforGEPF);
                applicationRepository.DetermineGEPFAttribute(application);
                application.CalculateApplicationDetail(false, false);
                applicationRepository.SaveApplication(application);
            }
        }
    }
}