using SAHL.Services.EventPublisher.Services.Models;

namespace SAHL.Services.EventPublisher.Services
{
    public interface IEventPublisherDataService
    {
        EventDataModel GetEventDataModelByEventKey(int eventKey);
    }
}