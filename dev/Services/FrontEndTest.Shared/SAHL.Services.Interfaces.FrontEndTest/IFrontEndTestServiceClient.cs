using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest
{
    public interface IFrontEndTestServiceClient
    {
        ISystemMessageCollection PerformQuery<T>(T query) where T : IFrontEndTestQuery;

        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFrontEndTestCommand;
    }
}