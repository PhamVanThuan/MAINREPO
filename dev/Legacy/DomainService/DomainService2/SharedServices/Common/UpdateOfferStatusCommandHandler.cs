using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class UpdateOfferStatusCommandHandler : IHandlesDomainServiceCommand<UpdateOfferStatusCommand>
    {
        private IApplicationRepository applicationRepository;

        public UpdateOfferStatusCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateOfferStatusCommand command)
        {
            applicationRepository.UpdateOfferStatusWithNoValidation(command.ApplicationKey, command.OfferStatusKey, command.OfferInformationTypeKey);
        }
    }
}