namespace DomainService2
{
    public interface IRuleSetDomainServiceCommand : IDomainServiceCommand
    {
        string WorkflowRuleSetName { get; }

        object[] RuleParameters { get; }

        bool Result { get; }
    }
}