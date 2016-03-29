using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.EventProjectionStreamer
{
    public class ProjectionUpdatedCommand : ServiceCommand, IEventProjectionStreamerCommand
    {
        public ProjectionUpdatedCommand(string projectionName, object projectionData)
        {
            if (string.IsNullOrWhiteSpace(projectionName)) { throw new ArgumentNullException("projectionName"); }
            if (projectionData == null) { throw new ArgumentNullException("projectionData"); }

            this.ProjectionName = projectionName;
            this.ProjectionData = projectionData;
        }

        public string ProjectionName { get; protected set; }
        public object ProjectionData { get; protected set; }
    }
}
