using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CreateCommissionableConsultantRoleCommandHandler : IHandlesDomainServiceCommand<CreateCommissionableConsultantRoleCommand>
    {
        private IApplicationRepository applicationRepository;
        private IOrganisationStructureRepository organisationStructureRepository;

        public CreateCommissionableConsultantRoleCommandHandler(IApplicationRepository applicationRepository, IOrganisationStructureRepository organisationStructureRepository)
        {
            this.applicationRepository = applicationRepository;
            this.organisationStructureRepository = organisationStructureRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, CreateCommissionableConsultantRoleCommand command)
        {
            var application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            var adUser = organisationStructureRepository.GetAdUserForAdUserName(command.ADUserName);
            application.AddRole((int)OfferRoleTypes.CommissionableConsultant, adUser.LegalEntity);
            applicationRepository.SaveApplication(application);
        }
    }
}