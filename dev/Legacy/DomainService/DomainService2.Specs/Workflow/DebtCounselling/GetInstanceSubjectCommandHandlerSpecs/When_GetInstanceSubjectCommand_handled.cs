using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.DebtCounselling.GetInstanceSubjectCommandHandlerSpecs
{
    [Subject(typeof(GetInstanceSubjectForDebtCounsellingCommandHandler))]
    public class When_GetInstanceSubjectCommand_handled : DomainServiceSpec<GetInstanceSubjectForDebtCounsellingCommand, GetInstanceSubjectForDebtCounsellingCommandHandler>
    {
        protected static IX2Repository x2repository;
        protected static string subject = "a test subject";
        Establish context = () =>
            {
                x2repository = An<IX2Repository>();
                x2repository.WhenToldTo(x => x.GetDebtCounsellingInstanceSubject(Param.IsAny<int>()))
                    .Return(subject);

                command = new GetInstanceSubjectForDebtCounsellingCommand(1);
                handler = new GetInstanceSubjectForDebtCounsellingCommandHandler(x2repository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_the_subject = () =>
            {
                command.LegalEntityNameResult.ShouldEqual(subject);
            };
    }
}