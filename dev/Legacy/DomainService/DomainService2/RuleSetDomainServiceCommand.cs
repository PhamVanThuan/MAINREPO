namespace DomainService2
{
    public abstract class RuleSetDomainServiceCommand : StandardDomainServiceCommand, DomainService2.IRuleSetDomainServiceCommand
    {
        public RuleSetDomainServiceCommand(bool ignoreWarnings, string workflowRuleSetName)
            : base(ignoreWarnings)
        {
            this.WorkflowRuleSetName = workflowRuleSetName;
        }

        public string WorkflowRuleSetName { get; protected set; }

        public virtual object[] RuleParameters { get; set; }

        public bool Result { get; set; }
    }
}