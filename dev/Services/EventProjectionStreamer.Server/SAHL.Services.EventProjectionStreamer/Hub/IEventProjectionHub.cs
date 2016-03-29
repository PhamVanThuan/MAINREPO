using Microsoft.AspNet.SignalR.Hubs;

namespace SAHL.Services.EventProjectionStreamer
{
    public interface IEventProjectionHub : IHub
    {
        void SubscribeToProjection(string projectionName);
        void UnsubscribeFromProjection(string projectionName);

        void ProjectionUpdated(string projectionName, dynamic projectionData);
    }
}