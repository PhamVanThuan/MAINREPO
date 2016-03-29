using System;
using DomainService2.Workflow.LoanAdjustments;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.LoanAdjustments
{
    [Subject(typeof(ApproveTermChangeRequestCommandHandler))]
    public class When_approving_term_change_using_invalid_accountkey : DomainServiceSpec<ApproveTermChangeRequestCommand, ApproveTermChangeRequestCommandHandler>
    {
        static IAccountRepository AccountRepo;
        static IMortgageLoanRepository mortgageLoanRepository;
        static IMortgageLoanAccount MortgageLoanAccount;
        static IMortgageLoan vml;
        static IAccount acc;
        static Exception exception;
        static ICommonRepository commonRepository;

        //Arrange
        Establish context = () =>
        {
            AccountRepo = An<IAccountRepository>();
            mortgageLoanRepository = An<IMortgageLoanRepository>();
            commonRepository = An<ICommonRepository>();

            MortgageLoanAccount = An<IMortgageLoanAccount>();
            vml = An<IMortgageLoan>();
            acc = An<IAccount>();

            AccountRepo.WhenToldTo(x => x.GetAccountByKey(Param<int>.IsAnything)).Return(MortgageLoanAccount);
            MortgageLoanAccount.WhenToldTo(x => x.SecuredMortgageLoan).Return(vml);

            command = new ApproveTermChangeRequestCommand(Param.IsAny<int>(), Param.IsAny<int>(), false);

            mortgageLoanRepository.WhenToldTo(x => x.TermChange(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything)).Throw(new Exception());

            handler = new ApproveTermChangeRequestCommandHandler(AccountRepo, mortgageLoanRepository, commonRepository);
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
    }
}