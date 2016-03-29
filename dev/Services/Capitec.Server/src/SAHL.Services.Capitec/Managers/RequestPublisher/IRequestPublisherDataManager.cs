using SAHL.Services.Capitec.Models.Shared;

namespace SAHL.Services.Capitec.Managers.RequestPublisher
{
    public interface IRequestPublisherDataManager
    {
        void AddPublishMessageFailure(SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication);

        void AddGenericMessage(SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication);
    }
}