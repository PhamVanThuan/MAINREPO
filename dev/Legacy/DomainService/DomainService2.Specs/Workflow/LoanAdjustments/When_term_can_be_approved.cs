﻿using System;


using DomainService2.Workflow.LoanAdjustments;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.LoanAdjustments
{
    [Subject(typeof(ApproveTermChangeRequestCommandHandler))]
    public class When__term_can_be_approved : DomainServiceSpec<ApproveTermChangeRequestCommand, ApproveTermChangeRequestCommandHandler>
    {
        static IAccountRepository AccountRepo;
        static IMortgageLoanRepository mortgageLoanRepository;
        static ICommonRepository commonRepository;

        //Arrange
        Establish context = () =>
        {
            AccountRepo = An<IAccountRepository>();
            mortgageLoanRepository = An<IMortgageLoanRepository>();
            commonRepository = An<ICommonRepository>();

            mortgageLoanRepository.WhenToldTo(x => x.TermChange(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<string>())).Throw(new Exception());

            command = new ApproveTermChangeRequestCommand(Param.IsAny<int>(), Param.IsAny<int>(), false);
            handler = new ApproveTermChangeRequestCommandHandler(AccountRepo, mortgageLoanRepository, commonRepository);
        };

        Because of = () =>
        {
            Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}