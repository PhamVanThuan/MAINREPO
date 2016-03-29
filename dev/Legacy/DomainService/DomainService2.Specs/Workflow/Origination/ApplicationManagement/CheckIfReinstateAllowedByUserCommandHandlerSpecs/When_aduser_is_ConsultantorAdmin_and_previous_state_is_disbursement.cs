using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckIfReinstateAllowedByUserCommandHandlerSpecs
{
    [Subject(typeof(CheckIfReinstateAllowedByUserCommandHandler))]
    public class When_aduser_is_ConsultantorAdmin_and_previous_state_is_disbursement : WithFakes
    {
        private static CheckIfReinstateAllowedByUserCommand command;
        private static CheckIfReinstateAllowedByUserCommandHandler handler;
        private static IDomainMessageCollection messages;

        private static IWorkflowAssignmentRepository workflowAssRepository;

        Establish context = () =>
            {
                workflowAssRepository = An<IWorkflowAssignmentRepository>();
                workflowAssRepository.WhenToldTo(x => x.IsUserInOrganisationStructureRole(Param.IsAny<string>(), Param.IsAny<IList<OfferRoleTypes>>()))
                                     .Return(true);

                command = new CheckIfReinstateAllowedByUserCommand(1, "Disbursement", false, "NishkarR");
                handler = new CheckIfReinstateAllowedByUserCommandHandler(workflowAssRepository);
                messages = new DomainMessageCollection();
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_an_error = () =>
            {
                messages[0].ShouldBeOfType(typeof(Error));
            };
    }
}