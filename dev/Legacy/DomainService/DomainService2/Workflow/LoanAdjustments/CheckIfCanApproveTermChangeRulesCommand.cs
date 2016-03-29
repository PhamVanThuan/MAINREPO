namespace DomainService2.Workflow.LoanAdjustments
{
    public class CheckIfCanApproveTermChangeRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckIfCanApproveTermChangeRulesCommand(int accountkey, long instanceID, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApproveTermChange)
        {
            this.AccountKey = accountkey;
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public int AccountKey { get; set; }
    }
}