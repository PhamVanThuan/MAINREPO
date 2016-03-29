using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class UpdateParentVarsCommandHandler : IHandlesDomainServiceCommand<UpdateParentVarsCommand>
    {
        private IX2WorkflowService x2WorkflowService;
        private IX2Repository x2Repository;

        public UpdateParentVarsCommandHandler(IX2WorkflowService x2WorkflowService, IX2Repository x2Repository)
        {
            this.x2WorkflowService = x2WorkflowService;
            this.x2Repository = x2Repository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateParentVarsCommand command)
        {
            IInstance iChild = x2Repository.GetInstanceByKey(command.ChildInstanceID);
            x2WorkflowService.SetX2DataRow(iChild.ParentInstance.ID, command.Dict);
        }
    }
}