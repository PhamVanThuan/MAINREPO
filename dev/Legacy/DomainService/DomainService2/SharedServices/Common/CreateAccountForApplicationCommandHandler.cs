using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.SharedServices.Common
{
    public class CreateAccountForApplicationCommandHandler : IHandlesDomainServiceCommand<CreateAccountForApplicationCommand>
    {
        private IApplicationRepository applicationRepository;
        private ICommonRepository commonRepository;

        public CreateAccountForApplicationCommandHandler(IApplicationRepository applicationRepository, ICommonRepository commonRepository)
        {
            this.applicationRepository = applicationRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, CreateAccountForApplicationCommand command)
        {
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application.ApplicationType.Key != (int)OfferTypes.ReAdvance &&
                application.ApplicationType.Key != (int)OfferTypes.FurtherAdvance)
            {
                applicationRepository.CreateAccountFromApplication(command.ApplicationKey, command.ADUserName);
                commonRepository.RefreshDAOObject<IApplication>(command.ApplicationKey);
            }
        }
    }
}