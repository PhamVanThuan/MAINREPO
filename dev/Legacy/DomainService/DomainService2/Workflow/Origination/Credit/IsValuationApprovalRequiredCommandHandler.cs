using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.Credit
{
    public class IsValuationApprovalRequiredCommandHandler : IHandlesDomainServiceCommand<IsValuationApprovalRequiredCommand>
    {
        IX2Repository x2Repo;

        public IsValuationApprovalRequiredCommandHandler(IX2Repository x2repository)
        {
            this.x2Repo = x2repository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, IsValuationApprovalRequiredCommand command)
        {
            command.Result = x2Repo.IsValuationApprovalRequired(command.InstanceID);
        }
    }
}