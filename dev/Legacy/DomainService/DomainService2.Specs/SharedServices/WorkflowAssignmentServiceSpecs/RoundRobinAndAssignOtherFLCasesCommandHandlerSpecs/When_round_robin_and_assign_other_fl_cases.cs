using DomainService2.SharedServices.WorkflowAssignment;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.SharedServices.WorkflowAssignmentServiceSpecs.RoundRobinAndAssignOtherFLCasesCommandHandlerSpecs
{
    [Subject(typeof(RoundRobinAndAssignOtherFLCasesCommandHandler))]
    public class When_round_robin_and_assign_other_fl_cases : DomainServiceSpec<RoundRobinAndAssignOtherFLCasesCommand, RoundRobinAndAssignOtherFLCasesCommandHandler>
    {
        static IDomainMessageCollection messages;
        static IWorkflowAssignmentRepository repo;
        static bool result;
        Establish context = () =>
        {
            result = false;
            repo = An<IWorkflowAssignmentRepository>();
            repo.WhenToldTo(x => x.IsUserActive(Param.IsAny<string>())).Return(result);
            messages = new DomainMessageCollection();
            command = new RoundRobinAndAssignOtherFLCasesCommand(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<int>());
            handler = new RoundRobinAndAssignOtherFLCasesCommandHandler(repo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_RoundRobinAndAssignOtherFLCasesCommandHandler_workflow_assignment_repo = () =>
        {
            repo.WasToldTo(x => x.RoundRobinAndAssignOtherFLCases(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<int>()));
        };
    }
}