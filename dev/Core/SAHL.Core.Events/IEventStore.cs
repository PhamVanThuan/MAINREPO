using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events
{
    internal interface IEventStore
    {
        void StoreEvent(DateTime eventOccuranceDate, IEvent eventToStore, int genericKey, int genericKeyTypeKey, IServiceRequestMetadata metadata);
    }
}