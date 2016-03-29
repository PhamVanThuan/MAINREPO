using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.PersonalLoan.GetADUserNameByWorkflowRoleTypeCommandHandlerSpecs
{
    [Subject(typeof(GetADUserNameByWorkflowRoleTypeCommandHandler))]
    public class When_workflow_roles_exist : DomainServiceSpec<GetADUserNameByWorkflowRoleTypeCommand, GetADUserNameByWorkflowRoleTypeCommandHandler>
    {
        private static IX2Repository x2Repository;
        private static IOrganisationStructureRepository organisationStructureRepository;
        private static string adUserName = @"SAHL\BumbumP";

        Establish context = () =>
            {
                var legalEntity = An<ILegalEntity>();
                legalEntity.WhenToldTo(c => c.Key).Return(1);

                var workflowRole = An<IWorkflowRole>();
                workflowRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

                x2Repository = An<IX2Repository>();
                x2Repository.WhenToldTo(x => x.GetWorkflowRoleForGenericKey(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>())).Return(new List<IWorkflowRole>() { workflowRole });

                var adUser = An<IADUser>();
                adUser.WhenToldTo(a => a.ADUserName).Return(adUserName);

                organisationStructureRepository = An<IOrganisationStructureRepository>();
                organisationStructureRepository.WhenToldTo(x => x.GetAdUserByLegalEntityKey(Param.IsAny<int>())).Return(adUser);

                command = new GetADUserNameByWorkflowRoleTypeCommand(1, 1);
                handler = new GetADUserNameByWorkflowRoleTypeCommandHandler(x2Repository, organisationStructureRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_the_ADUser_name = () =>
            {
                command.ADUserNameResult.ShouldBeTheSameAs(adUserName);
            };

    }
}
