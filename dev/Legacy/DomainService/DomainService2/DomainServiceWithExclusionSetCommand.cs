namespace DomainService2
{
    using System.Collections.Generic;
    using SAHL.Common.Globals;

    public class DomainServiceWithExclusionSetCommand : StandardDomainServiceCommand, IDomainServiceWithExclusionSetCommand
    {
        public DomainServiceWithExclusionSetCommand()
        {
            ExclusionSets = new List<RuleExclusionSets>();
        }

        public IList<RuleExclusionSets> ExclusionSets { get; protected set; }
    }
}