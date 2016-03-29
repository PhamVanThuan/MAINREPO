using System;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.GetReviewDateCommandHandlerSpecs
{
    [Subject(typeof(GetReviewDateCommandHandler))]
    public class When_get_reviewDate_with_no_accepted_active_proposal : WithFakes
    {
        protected static GetReviewDateCommand command;
        protected static GetReviewDateCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IDebtCounselling debtCounselling;
        protected static IProposal proposal;
        static Exception exception;

        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            debtCounselling = An<IDebtCounselling>();
            proposal = null;

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);
            debtCounselling.WhenToldTo(x => x.AcceptedActiveProposal).Return(proposal);

            messages = new DomainMessageCollection();
            command = new GetReviewDateCommand(1);
            handler = new GetReviewDateCommandHandler(debtCounsellingRepository);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_throw_exception = () =>
        {
            exception.ShouldBeOfType(typeof(Exception));
            exception.ShouldNotBeNull();
        };

        It should_return_null_reviewdate = () =>
        {
            command.ReviewDateResult.ShouldEqual<DateTime?>(null);
        };
    }
}