using SAHL.Core.Services;

namespace SAHL.Core.Data
{
    public interface ISqlServiceQuery<U> : IServiceQuery<IServiceQueryResult<U>>
    {
    }
}