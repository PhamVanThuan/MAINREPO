using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.Halo
{
    public interface IHaloServiceClient
    {
        ISystemMessageCollection PerformQuery<T>(T query) where T : IHaloServiceQuery;
    }
}