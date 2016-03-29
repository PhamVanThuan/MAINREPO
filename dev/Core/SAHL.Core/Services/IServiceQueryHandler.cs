using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IServiceQueryHandler<T> where T : IServiceQuery
    {
        ISystemMessageCollection HandleQuery(T query);
    }
}