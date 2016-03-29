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
    public class When_active_proposals_exist : DomainServiceSpec<UpdateActiveAcceptedProposalCommand, UpdateActiveAcceptedProposalCommandHandler>
    {
        static IDebtCounsellingRepository debtCounsellingRepository;
        static List<IProposal> activeProposals;
        static Exception exception;
        static IProposal proposal;

        // Arrange
        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            activeProposals = new List<IProposal>();
            proposal = An<IProposal>();
            activeProposals.Add(proposal);

            debtCounsellingRepository.WhenToldTo(x => x.GetProposalsByTypeAndStatus(Param<int>.IsAnything, Param<ProposalTypes>.IsAnything, Param<ProposalStatuses>.IsAnything)).Return(activeProposals);

            command = new UpdateActiveAcceptedProposalCommand(Param<int>.IsAnything, false);
            handler = new UpdateActiveAcceptedProposalCommandHandler(debtCounsellingRepository);
        };

        // Act
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        // Assert
        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };

        It should_not_throw_a_domainmessageexception = () =>
        {
            exception.ShouldBeNull();
        };

        // Assert
        It should_update_and_save_proposal_record = () =>
        {
            debtCounsellingRepository.WasToldTo(x => x.SaveProposal((Param<IProposal>.IsAnything)));
        };
    }
}