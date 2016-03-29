using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Cap2
{
    public class IsCreditCheckRequiredCommandHandler : IHandlesDomainServiceCommand<IsCreditCheckRequiredCommand>
    {
        private ICapRepository capRepository;

        public IsCreditCheckRequiredCommandHandler(ICapRepository capRepository)
        {
            this.capRepository = capRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsCreditCheckRequiredCommand command)
        {
            ICapApplication capApp = this.capRepository.GetCapOfferByKey(command.ApplicationKey);
            command.Result = this.capRepository.CheckLTVThreshold(capApp);
        }
    }
}