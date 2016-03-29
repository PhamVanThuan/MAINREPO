using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetLegalEntityLegalNameCommandHandler : IHandlesDomainServiceCommand<GetLegalEntityLegalNameCommand>
    {
        private ILegalEntityRepository legalEntityRepository;

        public GetLegalEntityLegalNameCommandHandler(ILegalEntityRepository legalEntityRepository)
        {
            this.legalEntityRepository = legalEntityRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetLegalEntityLegalNameCommand command)
        {
            string name = string.Empty;

            ILegalEntity legalEntity = legalEntityRepository.GetLegalEntityByKey(command.LegalEntityKey);

            command.LegalNameResult = legalEntity.GetLegalName(command.LegalNameFormat);
        }
    }
}