using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Cap2
{
    public class IsLANotRequiredCommandHandler : IHandlesDomainServiceCommand<IsLANotRequiredCommand>
    {
        private ICapRepository capRepository;

        public IsLANotRequiredCommandHandler(ICapRepository capRepository)
        {
            this.capRepository = capRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsLANotRequiredCommand command)
        {
            ICapApplication capApp = this.capRepository.GetCapOfferByKey(command.CapApplicationKey);
            command.Result = this.capRepository.IsReAdvanceLAA(capApp);
        }
    }
}