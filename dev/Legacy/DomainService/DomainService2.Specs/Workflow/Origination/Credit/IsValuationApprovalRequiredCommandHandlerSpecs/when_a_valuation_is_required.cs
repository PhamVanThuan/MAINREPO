using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Credit.IsValuationApprovalRequiredCommandHandlerSpecs
{
    [Subject(typeof(IsValuationApprovalRequiredCommandHandler))]
    public class when_a_valuation_is_required : WithFakes
    {
        protected static IsValuationApprovalRequiredCommand command;
        protected static IsValuationApprovalRequiredCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IX2Repository x2Repo;

        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                x2Repo = An<IX2Repository>();
                x2Repo.WhenToldTo(x => x.IsValuationApprovalRequired(Param<long>.IsAnything)).Return(true);

                command = new IsValuationApprovalRequiredCommand(Param<long>.IsAnything);
                handler = new IsValuationApprovalRequiredCommandHandler(x2Repo);
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