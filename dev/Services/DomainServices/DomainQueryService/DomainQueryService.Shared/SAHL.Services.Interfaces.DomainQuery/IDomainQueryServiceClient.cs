using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.DomainQuery
{
    public interface IDomainQueryServiceClient
    {
        ISystemMessageCollection PerformQuery<T>(T query) where T : IDomainQueryQuery;
    }
}