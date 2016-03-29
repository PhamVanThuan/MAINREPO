using SAHL.Core.Data;
using SAHL.Core.Services;

namespace SAHL.Core.Events.Projections
{
    public interface ITableProjector<TEvent, TDataModel> : IEventProjector<TEvent>
        where TEvent : IEvent
        where TDataModel : IDataModel
    {
        void Handle(TEvent @event, IServiceRequestMetadata metadata);
    }
}