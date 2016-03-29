using SAHL.X2Engine2.Tests.X2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Tests.X2.Managers
{
    public interface IX2TestDataManager
    {
        List<X2ScenarioMapInfo> GetApplicationCaptureTestCases(string hostName, int workerId);

        long GetRelatedInstanceIDFromParentInstance(long instanceID);

        X2Case GetX2Case(long instanceId);
    }
}
