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
    public class When_get_reviewDate : WithFakes
    {
        static GetReviewDateCommand command;
        static GetReviewDateCommandHandler handler;
        static IDomainMessageCollection messages;
        static DateTime? reviewDate;
        static IDebtCounsellingRepository debtCounsellingRepository;
        static IDebtCounselling debtCounselling;
        static IProposal proposal;
        static Exception exception;

        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            debtCounselling = An<IDebtCounselling>();
            proposal = An<IProposal>();

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);
            debtCounselling.WhenToldTo(x => x.AcceptedActiveProposal).Return(proposal);

            reviewDate = DateTime.Now;

            proposal.WhenToldTo(x => x.ReviewDate).Return(reviewDate);

            messages = new DomainMessageCollection();
            command = new GetReviewDateCommand(1);
            handler = new GetReviewDateCommandHandler(debtCounsellingRepository);
        };
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_not_throw_a_domainmessageexception = () =>
        {
            exception.ShouldBeNull();
        };

        It should_return_reviewdate = () =>
        {
            command.ReviewDateResult.ShouldEqual<DateTime?>(reviewDate);
        };
    }
}