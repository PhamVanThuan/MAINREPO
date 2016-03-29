using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;

namespace SAHL.Services.Interfaces.X2
{
    public interface IX2Service
    {
        ISystemMessageCollection PerformCommand<T>(T command) where T : IX2Message;
    }
}