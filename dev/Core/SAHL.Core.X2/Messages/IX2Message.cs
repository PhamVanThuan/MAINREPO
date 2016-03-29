using SAHL.Core.Services;

namespace SAHL.Core.X2.Messages
{
    public interface IX2Message : IServiceCommand, IServiceCommandWithReturnedData<X2Response>
    {
    }
}