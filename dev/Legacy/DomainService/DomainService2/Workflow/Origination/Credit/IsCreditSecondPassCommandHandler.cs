using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.Credit
{
    public class IsCreditSecondPassCommandHandler : IHandlesDomainServiceCommand<IsCreditSecondPassCommand>
    {
        IX2Repository x2Repo;

        public IsCreditSecondPassCommandHandler(IX2Repository x2repo)
        {
            this.x2Repo = x2repo;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, IsCreditSecondPassCommand command)
        {
            command.Result = x2Repo.HasRelatedSourceInstancesInWorkflow(command.InstanceID, SAHL.Common.Constants.WorkFlowName.Credit);
        }
    }
}