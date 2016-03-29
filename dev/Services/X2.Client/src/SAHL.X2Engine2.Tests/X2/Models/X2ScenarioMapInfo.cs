using System.Collections.Generic;

namespace SAHL.X2Engine2.Tests.X2.Models
{
    public class X2ScenarioMapInfo
    {
        public string ActivityName { get; private set; }
        public string ProcessName { get; private set; }
        public string WorkFlowName { get; private set; }
        public string UserName { get; private set; }
        public bool IgnoreWarnings { get; private set; }
        public Dictionary<string, string> MapVariables { get; private set; }
        public int SleepSeconds { get; private set; }

        public X2ScenarioMapInfo(string activityName, string processName, string workFlowName, string userName, bool ignoreWarnings, int sleepSeconds, Dictionary<string, string> mapVariables)
        {
            ActivityName = activityName;
            ProcessName = processName;
            WorkFlowName = workFlowName;
            UserName = userName;
            IgnoreWarnings = ignoreWarnings;
            MapVariables = mapVariables;
            SleepSeconds = sleepSeconds * 1000;
        }
    }
}