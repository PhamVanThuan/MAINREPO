using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Credit.IsCreditSecondPassCommandHandlerSpecs
{
    [Subject(typeof(IsCreditSecondPassCommandHandler))]
    public class when_instance_has_no_source_instance_in_workflow : WithFakes
    {
        protected static IsCreditSecondPassCommand command;
        protected static IsCreditSecondPassCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IX2Repository x2Repo;

        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                x2Repo = An<IX2Repository>();
                x2Repo.WhenToldTo(x => x.HasRelatedSourceInstancesInWorkflow(Param<long>.IsAnything, Param<string>.IsAnything)).Return(true);

                command = new IsCreditSecondPassCommand(Param<long>.IsAnything);
                handler = new IsCreditSecondPassCommandHandler(x2Repo);
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