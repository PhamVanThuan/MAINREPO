using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IServiceQueryRule : IRule
    {
    }

    public interface IServiceQueryRule<T> : IServiceQueryRule where T : IServiceQuery
    {
        ISystemMessageCollection ExecuteRule(T query);
    }
}