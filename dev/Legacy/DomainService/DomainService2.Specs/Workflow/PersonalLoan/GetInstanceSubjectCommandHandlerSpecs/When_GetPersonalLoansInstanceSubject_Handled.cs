using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.PersonalLoan.GetInstanceSubjectCommandHandlerSpecs
{
    [Subject(typeof(GetInstanceSubjectForPersonalLoanCommandHandler))]
    public class When_GetPersonalLoansInstanceSubject_Handled : DomainServiceSpec<GetInstanceSubjectForPersonalLoanCommand, GetInstanceSubjectForPersonalLoanCommandHandler>
    {
        private static IX2Repository x2Repository;
        protected static string subject = "a test subject";
        Establish context = () =>
        {
            x2Repository = An<IX2Repository>();
            command = new GetInstanceSubjectForPersonalLoanCommand(Param.IsAny<int>());
            handler = new GetInstanceSubjectForPersonalLoanCommandHandler(x2Repository);

            x2Repository.WhenToldTo(x => x.GetPersonalLoansInstanceSubject(Param.IsAny<int>())).Return(subject);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_the_subject = () =>
        {
            command.Result.ShouldEqual(subject);
        };
    }
}