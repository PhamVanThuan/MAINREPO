namespace DomainService2
{
    public abstract class RuleDomainServiceCommand : StandardDomainServiceCommand, IRuleDomainServiceCommand
    {
        public RuleDomainServiceCommand(bool ignoreWarnings, string workflowRuleName)
            : base(ignoreWarnings)
        {
            this.WorkflowRuleName = workflowRuleName;
        }

        public string WorkflowRuleName { get; protected set; }

        public virtual object[] RuleParameters { get; set; }

        public bool Result { get; set; }
    }
}