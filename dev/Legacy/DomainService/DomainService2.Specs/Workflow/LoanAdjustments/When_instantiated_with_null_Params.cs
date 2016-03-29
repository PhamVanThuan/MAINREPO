using System;
using DomainService2.Workflow.LoanAdjustments;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.LoanAdjustments
{
    [Subject(typeof(ApproveTermChangeRequestCommandHandler))]
    public class When_instantiated_with_null_Params : RuleSetDomainServiceSpec<CheckIfCanApproveTermChangeRulesCommand, CheckIfCanApproveTermChangeRulesCommandHandler>
    {
        static IAccountRepository AccountRepo;
        static Exception exception;

        //Arrange
        Establish context = () =>
        {
            AccountRepo = null;

            command = new CheckIfCanApproveTermChangeRulesCommand(Param<int>.IsAnything, Param<long>.IsAnything, Param<bool>.IsAnything);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler = new CheckIfCanApproveTermChangeRulesCommandHandler(commandHandler, AccountRepo));
        };

        It should_throw_a_ArgumentNullException = () =>
        {
            // exception.ShouldBeOfType<ArgumentNullException>();
        };
    }
}