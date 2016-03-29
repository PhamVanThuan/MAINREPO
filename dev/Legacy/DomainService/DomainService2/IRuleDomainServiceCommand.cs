namespace DomainService2
{
    public interface IRuleDomainServiceCommand : IDomainServiceCommand
    {
        string WorkflowRuleName { get; }

        object[] RuleParameters { get; }

        bool Result { get; }
    }
}