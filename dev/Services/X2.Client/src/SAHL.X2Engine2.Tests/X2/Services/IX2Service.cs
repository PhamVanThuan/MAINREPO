using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Principal;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using SAHL.X2Engine2.Tests.X2;
using SAHL.X2Engine2.Tests.X2.Models;
namespace SAHL.X2Engine2.Tests.X2.Services
{
    public interface IX2Service : IDisposable
    {
        X2Case CreateWorkflowInstance(X2ProcessWorkflow process, string requestByUser, string activityName);
        void RaiseExternalFlag(X2ProcessWorkflow process, long instanceId,X2ExternalActivity x2ExternalActivity);
    }
}
