using SAHL.Core.X2.Messages;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Providers
{
    public interface IX2EngineConfigurationProvider
    {
        string GetHostName();

        int GetRequestTimeoutInMilliseconds();

        int GetTimeToWaitUntilSchedulingActivities();

        string GetSAHLNugetServerURL();

        string GetNuGetCachePath();

        string GetNuGetCacheEnvironmentVariableName();

        bool PublishingNode();

        string LogFilePath();

        bool EnableLogging();

        IEnumerable<X2Workflow> GetSupportedWorkflows();

        int GetNumberOfResponseConsumers();

        int GetNumberOfManagementConsumers();
    }
}