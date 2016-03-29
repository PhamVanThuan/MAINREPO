using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IServiceQueryRouter
    {
        ISystemMessageCollection HandleQuery<T>(T command) where T : IServiceQuery;

        ISystemMessageCollection HandleQuery(object command);
    }
}