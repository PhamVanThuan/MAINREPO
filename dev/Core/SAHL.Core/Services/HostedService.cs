using System;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.Services
{
    public class HostedService : IHostedService
    {
        private string serviceName;

        public HostedService()
        {
            // find the service domain config project to get the appropriate assembly
            Assembly serviceCfg = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("SAHL.Config.Services.")
                && !x.FullName.StartsWith("SAHL.Config.Services.Core")
                && !x.FullName.StartsWith("SAHL.Config.Services.Windows")
                && !x.FullName.StartsWith("SAHL.Config.Services.Web")
                && !x.FullName.StartsWith("SAHL.Config.Services.dll")
                && !x.FullName.StartsWith("SAHL.Config.Services.Core.dll")
                && !x.ManifestModule.Name.EndsWith(".Client.dll")).SingleOrDefault();

            if (serviceCfg == null)
            {
                serviceName = "Unknown";
            }
            else
            {
                serviceName = serviceCfg.GetName().Name.Trim().Replace("SAHL.Config.Services.", "").Replace(".Server", "").Replace(".dll", "");
            }

            string serviceAssemblyName = string.Format("SAHL.Services.{0},", serviceName);
            Assembly domainSvc = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith(serviceAssemblyName)).SingleOrDefault();
            if (domainSvc != null)
            {
                this.Version = domainSvc.GetName().Version.ToString();
            }
            else
            {
                this.Version = "Unknown";
            }
        }

        public virtual void Start()
        {
            this.RunningStatus = HostedServiceRunningStatus.Starting;

            this.RunningStatus = HostedServiceRunningStatus.Started;
        }

        public virtual void Stop()
        {
            this.RunningStatus = HostedServiceRunningStatus.Stopping;

            this.RunningStatus = HostedServiceRunningStatus.Stopped;
        }

        public string Name
        {
            get { return this.serviceName; }
        }

        public HostedServiceRunningStatus RunningStatus
        {
            get;
            protected set;
        }

        public string Version
        {
            /// TODO: FIX THIS VERSION THING
            get;
            protected set;
        }
    }
}