using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class SendSMSToMainApplicantsCommandHandler : IHandlesDomainServiceCommand<SendSMSToMainApplicantsCommand>
    {
        private IMessageService messageService;
        private IApplicationRepository applicationRepository;

        public SendSMSToMainApplicantsCommandHandler(IMessageService messageService, IApplicationRepository applicationRepository)
        {
            this.messageService = messageService;
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, SendSMSToMainApplicantsCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            IReadOnlyEventList<ILegalEntity> legalEntities = application.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.MainApplicant });
            foreach (ILegalEntity legalEntity in legalEntities)
            {
                if (!string.IsNullOrEmpty(legalEntity.CellPhoneNumber))
                {
                    messageService.SendSMS(command.ApplicationKey, command.Message, legalEntity.CellPhoneNumber);
                }
            }
        }
    }
}