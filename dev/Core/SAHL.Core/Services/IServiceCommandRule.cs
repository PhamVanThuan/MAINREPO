using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IServiceCommandRule : IRule
    {
    }

    public interface IServiceCommandRule<T> : IServiceCommandRule where T : IServiceCommand
    {
        ISystemMessageCollection ExecuteRule(T query);
    }
}