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
    public class When_workflow_roles_do_not_exist : DomainServiceSpec<GetADUserNameByWorkflowRoleTypeCommand, GetADUserNameByWorkflowRoleTypeCommandHandler>
    {
        private static IX2Repository x2Repository;
        private static IOrganisationStructureRepository organisationStructureRepository;

        Establish context = () =>
        {
            x2Repository = An<IX2Repository>();
            x2Repository.WhenToldTo(x => x.GetWorkflowRoleForGenericKey(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>())).Return(new List<IWorkflowRole>() {  });

            organisationStructureRepository = An<IOrganisationStructureRepository>();

            command = new GetADUserNameByWorkflowRoleTypeCommand(1, 1);
            handler = new GetADUserNameByWorkflowRoleTypeCommandHandler(x2Repository, organisationStructureRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_empty_ADUser_name = () =>
        {
            string.IsNullOrEmpty(command.ADUserNameResult).ShouldBeTrue();
        };
    }
}
