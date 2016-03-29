using SAHL.Core.SystemMessages;

namespace SAHL.Core.Rules
{
    public interface IDomainRule<T> where T : class
    {
        void ExecuteRule(ISystemMessageCollection messages, T ruleModel);
    }
}