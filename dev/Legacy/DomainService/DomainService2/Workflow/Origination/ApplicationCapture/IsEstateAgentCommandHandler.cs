using SAHL.Common.Globals;
using X2DomainService.Interface.Common;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class IsEstateAgentCommandHandler : IHandlesDomainServiceCommand<IsEstateAgentCommand>
    {
        private ICommon commonWorkflowService;

        public IsEstateAgentCommandHandler(ICommon commonWorkflowService)
        {
            this.commonWorkflowService = commonWorkflowService;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, IsEstateAgentCommand command)
        {
            command.Result = commonWorkflowService.CheckIfUserIsPartOfOrgStructure(messages, OrganisationStructure.EstateAgent, command.CreatorADUserName);
        }
    }
}