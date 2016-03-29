using DomainService2.SharedServices.WorkflowAssignment;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.SharedServices.WorkflowAssignmentServiceSpecs.IsUserActiveByADUserKeyCommandHandlerSpecs
{
    [Subject(typeof(IsUserActiveByADUserKeyCommandHandler))]
    internal class when_user_active_by_aduserkey : WithFakes
    {
        static IsUserActiveByADUserKeyCommand command;
        static IsUserActiveByADUserKeyCommandHandler handler;
        static IDomainMessageCollection messages;
        static IWorkflowAssignmentRepository repo;
        static bool result;
        Establish context = () =>
            {
                result = false;
                repo = An<IWorkflowAssignmentRepository>();
                repo.WhenToldTo(x => x.IsUserActive(Param.IsAny<string>())).Return(result);
                messages = new DomainMessageCollection();
                command = new IsUserActiveByADUserKeyCommand(1234);
                handler = new IsUserActiveByADUserKeyCommandHandler(repo);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_true = () =>
            {
                command.Result.ShouldEqual(result);
            };

        It should_call_isUserActive_workflow_assignment_repo = () =>
            {
                repo.WasToldTo(x => x.IsUserActive(Param.IsAny<int>()));
            };
    }
}