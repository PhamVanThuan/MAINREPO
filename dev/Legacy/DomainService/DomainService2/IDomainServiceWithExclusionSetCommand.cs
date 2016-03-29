namespace DomainService2
{
    using System.Collections.Generic;
    using SAHL.Common.Globals;

    public interface IDomainServiceWithExclusionSetCommand : IDomainServiceCommand
    {
        IList<RuleExclusionSets> ExclusionSets { get; }
    }
}