using SAHL.Common.Collections.Interfaces;

namespace DomainService2
{
    public interface ICommandHandler
    {
        void HandleCommand<T>(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, T command) where T : IDomainServiceCommand;

        bool CheckRules<T>(IDomainMessageCollection messages, T command) where T : RuleSetDomainServiceCommand;

        bool CheckRule<T>(IDomainMessageCollection messages, T command) where T : RuleDomainServiceCommand;
    }
}