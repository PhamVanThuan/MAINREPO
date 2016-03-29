using System;
using System.Collections.Generic;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.DebtCounselling.UpdateActiveAcceptedProposalCommandHandlerSpces
{
    [Subject(typeof(UpdateActiveAcceptedProposalCommandHandler))]
    public class When_no_active_proposals_exist : DomainServiceSpec<UpdateActiveAcceptedProposalCommand, UpdateActiveAcceptedProposalCommandHandler>
    {
        static IDebtCounsellingRepository debtCounsellingRepository;
        static List<IProposal> activeProposals;
        static Exception exception;

        // Arrange
        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            activeProposals = new List<IProposal>();

            debtCounsellingRepository.WhenToldTo(x => x.GetProposalsByTypeAndStatus(Param<int>.IsAnything, Param<ProposalTypes>.IsAnything, Param<ProposalStatuses>.IsAnything)).Return(activeProposals);

            command = new UpdateActiveAcceptedProposalCommand(Param<int>.IsAnything, false);
            handler = new UpdateActiveAcceptedProposalCommandHandler(debtCounsellingRepository);
        };

        // Act
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_throw_exception = () =>
        {
            exception.ShouldBeOfType(typeof(Exception));
            exception.ShouldNotBeNull();
        };

        // Assert
        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };

        // Assert
        It should_not_update_and_save_proposal_record = () =>
        {
            debtCounsellingRepository.WasNotToldTo(x => x.SaveProposal((Param<IProposal>.IsAnything)));
        };
    }
}