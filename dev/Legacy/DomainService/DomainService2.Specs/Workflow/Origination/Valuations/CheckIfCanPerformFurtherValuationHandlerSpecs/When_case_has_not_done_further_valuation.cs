using DomainService2.SharedServices;
using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CheckIfCanPerformFurtherValuation
{
    [Subject(typeof(CheckIfCanPerformFurtherValuationCommandHandler))]
    public class When_case_has_not_done_further_valuation : CheckIfCanPerformFurtherValuationCommandHandlerBase
    {
        private static IX2WorkflowService x2WorkflowService;

        Establish context = () =>
            {
                x2WorkflowService = An<IX2WorkflowService>();
                x2WorkflowService.WhenToldTo(x => x.HasInstancePerformedActivity(Param.IsAny<long>(), Param.IsAny<string>()))
                                 .Return(false);

                messages = new DomainMessageCollection();
                command = new CheckIfCanPerformFurtherValuationCommand(1);
                handler = new CheckIfCanPerformFurtherValuationCommandHandler(x2WorkflowService);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_true = () =>
            {
                command.Result.ShouldBeTrue();
            };
    }
}