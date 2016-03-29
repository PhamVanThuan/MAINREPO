using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Events
    {
        public Events(List<EventModel> events, ServiceModel service)
        {
            this.EventModels = events;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public List<EventModel> EventModels { get; protected set; }
    }
}