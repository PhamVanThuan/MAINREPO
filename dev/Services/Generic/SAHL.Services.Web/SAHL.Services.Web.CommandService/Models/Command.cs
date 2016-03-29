using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

using SAHL.Core.Services.Metrics;

namespace SAHL.Services.Web.CommandService.Models
{
    public class Command
    {
        public Command(ICommandServiceRequestMetrics commandServiceMetrics, string commandName)
        {
            this.CommandServiceMetrics = commandServiceMetrics;
            this.CommandName           = commandName;
        }

        public string CommandName { get; protected set; }
        public ICommandServiceRequestMetrics CommandServiceMetrics { get; protected set; }
    }
}