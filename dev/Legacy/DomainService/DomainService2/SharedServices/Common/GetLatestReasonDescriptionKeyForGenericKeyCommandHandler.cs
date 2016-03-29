using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetLatestReasonDescriptionKeyForGenericKeyCommandHandler : IHandlesDomainServiceCommand<GetLatestReasonDescriptionKeyForGenericKeyCommand>
    {
        private IReasonRepository reasonRepository;

        public GetLatestReasonDescriptionKeyForGenericKeyCommandHandler(IReasonRepository reasonRepository)
        {
            this.reasonRepository = reasonRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetLatestReasonDescriptionKeyForGenericKeyCommand command)
        {
            command.Result = reasonRepository.GetLatestReasonDescriptionKeyForGenericKey(command.GenericKey, command.GenericKeyTypeKey);
        }
    }
}