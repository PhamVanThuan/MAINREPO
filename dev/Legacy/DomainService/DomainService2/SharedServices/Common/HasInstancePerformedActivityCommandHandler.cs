using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class HasInstancePerformedActivityCommandHandler : IHandlesDomainServiceCommand<HasInstancePerformedActivityCommand>
    {
        private IX2WorkflowService x2WorkflowService;

        public HasInstancePerformedActivityCommandHandler(IX2WorkflowService x2WorkflowService)
        {
            this.x2WorkflowService = x2WorkflowService;
        }

        public void Handle(IDomainMessageCollection messages, HasInstancePerformedActivityCommand command)
        {
            command.Result = this.x2WorkflowService.HasInstancePerformedActivity(command.InstanceID, command.Activity);
        }
    }
}