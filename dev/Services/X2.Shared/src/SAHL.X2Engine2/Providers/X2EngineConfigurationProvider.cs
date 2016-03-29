using System;
using System.Configuration;
using SAHL.Core.Configuration;
using SAHL.Core.X2.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Providers
{
    public class X2EngineConfigurationProvider : IX2EngineConfigurationProvider
    {
        public AppSettingsSection appSettingsSection;
        private ISerializationProvider serializationProvider;
        private IWorkflowDataProvider workflowDataProvider;
        private IEnumerable<X2Workflow> supportedWorkflows;

        public X2EngineConfigurationProvider(IConfigurationProvider config, ISerializationProvider serializationProvider, IWorkflowDataProvider workflowDataProvider)
        {
            this.appSettingsSection = config.Config.AppSettings.CurrentConfiguration.AppSettings;
            this.serializationProvider = serializationProvider;
            this.workflowDataProvider = workflowDataProvider;
        }

        public string GetHostName()
        {
            return appSettingsSection.Settings["messageBusServer"].Value;
        }

        public int GetRequestTimeoutInMilliseconds()
        {
            return Convert.ToInt32(appSettingsSection.Settings["RequestTimeoutInMilliseconds"].Value);
        }

        public int GetTimeToWaitUntilSchedulingActivities()
        {
            return Convert.ToInt32(appSettingsSection.Settings["TimeToWaitUntilSchedulingActivities"].Value);
        }

        public string GetSAHLNugetServerURL()
        {
            return appSettingsSection.Settings["NugetServerLocations"].Value;
        }

        public string GetNuGetCachePath()
        {
            return appSettingsSection.Settings["NuGetCachePath"].Value;
        }

        public string GetNuGetCacheEnvironmentVariableName()
        {
            return appSettingsSection.Settings["NuGetCacheEnvironmentVariableName"].Value;
        }

        public bool PublishingNode()
        {
            return Convert.ToBoolean(appSettingsSection.Settings["PublishingNode"].Value);
        }

        public string LogFilePath()
        {
            return appSettingsSection.Settings["LogFilePath"].Value;
        }

        public bool EnableLogging()
        {
            return Convert.ToBoolean(appSettingsSection.Settings["EnableLogging"].Value);
        }

        public int GetNumberOfResponseConsumers()
        {
            return Convert.ToInt32(appSettingsSection.Settings["ResponseConsumers"].Value);
        }

        public int GetNumberOfManagementConsumers()
        {
            return Convert.ToInt32(appSettingsSection.Settings["ManagementConsumers"].Value);
        }

        public IEnumerable<X2Workflow> GetSupportedWorkflows()
        {
            if (supportedWorkflows == null)
            {
                string jSon = ConfigurationManager.AppSettings["supportedProcesses"];
                List<X2Process> supportedProcesses = serializationProvider.Deserialize<List<X2Process>>(jSon);
                var supportedProcessNames = supportedProcesses.Select(x => x.ProcessName);
                var workflows = this.workflowDataProvider.GetSupportedWorkflows(supportedProcessNames);
                supportedWorkflows = workflows.Select(x => new X2Workflow(x.ProcessName, x.WorkflowName));
            }
            return supportedWorkflows;
        }
    }
}