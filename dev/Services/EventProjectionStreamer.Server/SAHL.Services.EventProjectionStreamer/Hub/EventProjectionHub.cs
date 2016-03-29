using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SAHL.Services.EventProjectionStreamer
{
    [HubName("eventProjectionHub")]
    public class EventProjectionHub : Hub<IEventProjectionHub>, IEventProjectionHub
    {
        public IHubCallerConnectionContext<dynamic> Clients { get; set; }

        [HubMethodName("subscribeToProjection")]
        public void SubscribeToProjection(string projectionName)
        {
            if (string.IsNullOrWhiteSpace(projectionName))
            {
                throw new ArgumentNullException("projectionName");
            }

            this.Groups.Add(this.Context.ConnectionId, projectionName);
        }

        public void UnsubscribeFromProjection(string projectionName)
        {
            if (string.IsNullOrWhiteSpace(projectionName))
            {
                throw new ArgumentNullException("projectionName");
            }

            this.Groups.Remove(this.Context.ConnectionId, projectionName);
        }

        public void ProjectionUpdated(string projectionName, dynamic projectionData)
        {
            if (string.IsNullOrWhiteSpace(projectionName))
            {
                throw new ArgumentNullException("projectionName");
            }
            if (projectionData == null) { throw new ArgumentNullException("projectionData"); }

            var projectionClientGroup = this.Clients.Group(projectionName);
            projectionClientGroup.updateProjection(projectionName, projectionData);
        }
    }
}
