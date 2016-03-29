using SAHL.Services.Capitec.Models.Shared;

namespace SAHL.Services.Capitec.Managers.RequestPublisher
{
    public interface IRequestPublisherManager
    {
        bool PublishWithRetry(SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication);
    }
}