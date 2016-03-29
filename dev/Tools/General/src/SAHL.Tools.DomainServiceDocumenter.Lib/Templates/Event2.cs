using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Event
    {
        public Event(List<EventModel> events, EventModel @event, ServiceModel service)
        {
            this.EventModels = events;

            this.EventModel = @event;

            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public EventModel EventModel { get; protected set; }

        public List<EventModel> EventModels { get; protected set; }
    }
}
