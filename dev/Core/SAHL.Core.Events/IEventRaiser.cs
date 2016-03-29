using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events
{
    public interface IEventRaiser
    {
        void RaiseEvent(DateTime eventOccuranceDate, IEvent @event, int genericKey, int genericKeyTypeKey, IServiceRequestMetadata metadata);
    }
}