using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.Credit
{
    public class UpdateConditionsCommandHandler : IHandlesDomainServiceCommand<UpdateConditionsCommand>
    {
        private IConditionsRepository conditionsRepo;

        public UpdateConditionsCommandHandler(IConditionsRepository conditionsRepo)
        {
            this.conditionsRepo = conditionsRepo;
        }

        public void Handle(IDomainMessageCollection messages, UpdateConditionsCommand command)
        {
            conditionsRepo.UpdateLoanConditions(command.ApplicationKey, (int)GenericKeyTypes.Offer);
        }
    }
}