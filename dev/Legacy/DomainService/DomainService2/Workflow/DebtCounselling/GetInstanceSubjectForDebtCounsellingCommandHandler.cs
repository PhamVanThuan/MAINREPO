using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.DebtCounselling
{
    public class GetInstanceSubjectForDebtCounsellingCommandHandler : IHandlesDomainServiceCommand<GetInstanceSubjectForDebtCounsellingCommand>
    {
        private IX2Repository x2repository;

        public GetInstanceSubjectForDebtCounsellingCommandHandler(IX2Repository x2repository)
        {
            this.x2repository = x2repository;
        }

        public void Handle(IDomainMessageCollection messages, GetInstanceSubjectForDebtCounsellingCommand command)
        {
            command.LegalEntityNameResult = this.x2repository.GetDebtCounsellingInstanceSubject(command.DebtCounsellingKey);
        }
    }
}