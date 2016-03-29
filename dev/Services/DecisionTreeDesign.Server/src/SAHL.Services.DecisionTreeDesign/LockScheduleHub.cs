using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using StructureMap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign
{
    [HubName("lockScheduleHub")]
    [Authorize]
    public class LockScheduleHub : Hub
    {
    }
}
