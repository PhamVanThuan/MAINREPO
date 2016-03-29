using System;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.RequestPublisher
{
    public interface IConfigurationProvider
    {
        int NumberOfTimesToRetryCreateApplication { get; }
    }
}