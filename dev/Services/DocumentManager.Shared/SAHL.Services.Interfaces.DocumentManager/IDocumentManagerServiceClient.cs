using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.DocumentManager
{
    public interface IDocumentManagerServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IDocumentManagerCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IDocumentManagerQuery;
    }
}