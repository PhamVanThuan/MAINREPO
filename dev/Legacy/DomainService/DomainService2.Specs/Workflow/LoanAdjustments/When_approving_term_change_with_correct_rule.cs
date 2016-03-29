using DomainService2.Workflow.LoanAdjustments;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.LoanAdjustments
{
    public class When_approving_term_change_with_correct_rule : RuleSetDomainServiceSpec<CheckIfCanApproveTermChangeRulesCommand, CheckIfCanApproveTermChangeRulesCommandHandler>
    {
        static IAccountRepository AccountRepo;

        protected const string ruleSet = "Approve Term Change";
        static IMortgageLoanAccount MortgageLoanAccount;
        static IMortgageLoan vml;
        static IAccount acc;

        //Arrange
        Establish context = () =>
        {
            AccountRepo = An<IAccountRepository>();
            MortgageLoanAccount = An<IMortgageLoanAccount>();
            vml = An<IMortgageLoan>();
            acc = An<IAccount>();

            AccountRepo.WhenToldTo(x => x.GetAccountByKey(Param<int>.IsAnything)).Return(MortgageLoanAccount);
            MortgageLoanAccount.WhenToldTo(x => x.SecuredMortgageLoan).Return(vml);
            command = new CheckIfCanApproveTermChangeRulesCommand(1, 1, false);
            handler = new CheckIfCanApproveTermChangeRulesCommandHandler(commandHandler, AccountRepo);
            command.InstanceID = 1;
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
       {
           command.RuleParameters[0].Equals(vml);

           command.RuleParameters[0].ShouldBeOfType(typeof(IMortgageLoan));

           command.RuleParameters[1].ShouldBeOfType(typeof(long));
       };

        It should_set_workflow_ruleset_to_approve_term_change = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(SAHL.Common.RuleSets.ApproveTermChange);
        };
    }
}