using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using X2DomainService.Interface.Common;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class IsEstateAgentInRoleCommandHandler : IHandlesDomainServiceCommand<IsEstateAgentInRoleCommand>
    {
        private ICommon commonWorkflowService;
        private IOrganisationStructureRepository organisationStructureRepository;

        public IsEstateAgentInRoleCommandHandler(ICommon commonWorkflowService,
                                                IOrganisationStructureRepository organisationStructureRepository)
        {
            this.commonWorkflowService = commonWorkflowService;
            this.organisationStructureRepository = organisationStructureRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, IsEstateAgentInRoleCommand command)
        {
            bool isEstateAgent = commonWorkflowService.CheckIfUserIsPartOfOrgStructure(messages, SAHL.Common.Globals.OrganisationStructure.EstateAgent, command.Username);
            if (isEstateAgent)
            {
                IADUser adUser = organisationStructureRepository.GetAdUserForAdUserName(command.Username);
                command.Result = adUser.UserOrganisationStructure.Any(x => command.Roles.Contains(x.OrganisationStructure.Description));
            }
        }
    }
}