using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.PersonalLoan
{
    public class GetADUserNameByWorkflowRoleTypeCommandHandler : IHandlesDomainServiceCommand<GetADUserNameByWorkflowRoleTypeCommand>
    {
        IX2Repository x2Repository;
        IOrganisationStructureRepository organisationStructureRepository;

        public GetADUserNameByWorkflowRoleTypeCommandHandler(IX2Repository x2Repository, IOrganisationStructureRepository organisationStructureRepository)
        {
            this.x2Repository = x2Repository;
            this.organisationStructureRepository = organisationStructureRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetADUserNameByWorkflowRoleTypeCommand command)
        {
            IList<IWorkflowRole> workflowRoles = x2Repository.GetWorkflowRoleForGenericKey(command.ApplicationKey, command.WorkflowRoleTypeKey, (int)SAHL.Common.Globals.GeneralStatuses.Active);
            if (workflowRoles != null && workflowRoles.Count > 0)
            {
                int legalEntityKey = workflowRoles[0].LegalEntity.Key;
                IADUser aduser = organisationStructureRepository.GetAdUserByLegalEntityKey(legalEntityKey);
                command.ADUserNameResult = aduser.ADUserName;
            }
        }
    }
}