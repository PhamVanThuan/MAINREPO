using System;
using System.ServiceModel;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using SAHL.Core;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Services.DomainProcessManager.Configuration;
using System.Diagnostics;

namespace SAHL.Services.DomainProcessManager.WcfService
{
    public class WcfServiceStartup : IStartable, IStoppable
    {
        private readonly IIocContainer iocContainer;
        private readonly IRawLogger rawLogger;
        private readonly ILoggerSource loggerSource;
        private readonly ILoggerAppSource loggerAppSource;
        private readonly IDictionary<string, ServiceHost> wcfServices = new Dictionary<string, ServiceHost>();

        public WcfServiceStartup(IIocContainer iocContainer, IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            if (rawLogger == null) { throw new ArgumentNullException("rawLogger"); }
            if (loggerSource == null) { throw new ArgumentNullException("loggerSource"); }
            if (loggerAppSource == null) { throw new ArgumentNullException("loggerAppSource"); }

            this.iocContainer    = iocContainer;
            this.rawLogger       = rawLogger;
            this.loggerSource    = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        public void Start()
        {
            this.wcfServices.Clear();

            var wcfservicesSection = (WcfServicesSection) ConfigurationManager.GetSection("WcfServices");
            if (wcfservicesSection == null) { return; }

            foreach (ServiceElement currentWcfService in wcfservicesSection.Services)
            {
                try
                {
                    var wcfServiceType    = this.GetWcfServiceInterface(currentWcfService);
                    var wcfServiceAddress = new Uri(currentWcfService.Address);
                    var wcfService        = this.GetWcfServiceObject(wcfServiceType);

                    this.LogMessage(string.Format("Starting  {0} WCF Service", currentWcfService.Name));
                    var serviceHost = this.StartWcfService(wcfService, wcfServiceAddress);
                    if (serviceHost == null) { continue; }

                    this.wcfServices.Add(currentWcfService.Name, serviceHost);
                    this.LogMessage(string.Format("{0} WCF Service Started successfully", currentWcfService.Name));
                }
                catch (Exception runtimeException)
                {
                    this.LogMessage(string.Format("Failed to start the {0} WCF Service", currentWcfService.Name), runtimeException);
                }
            }
        }

        public void Stop()
        {
            if (this.wcfServices == null) { return; }

            foreach (var currentWcfService in this.wcfServices)
            {
                currentWcfService.Value.Close();
                this.LogMessage(string.Format("{0} WCF Service Stopped successfully", currentWcfService.Key));
            }
        }

        protected virtual ServiceHost StartWcfService(object wcfService, Uri wcfServiceAddress)
        {
            var serviceHost = new ServiceHost(wcfService, wcfServiceAddress);
            serviceHost.Open();

            return serviceHost;
        }

        private Type GetWcfServiceInterface(ServiceElement currentWcfService)
        {
            if (string.IsNullOrEmpty(currentWcfService.Interface))
            {
                throw new ArgumentException(string.Format("{0} WCF Service Interface is not specified", currentWcfService.Name));
            }

            var wcfInterfaceType = Type.GetType(currentWcfService.Interface);
            if (wcfInterfaceType == null)
            {
                throw new ArgumentException(string.Format("Invalid Interface specified for {0} WCF Service", currentWcfService.Name));
            }

            return wcfInterfaceType;
        }

        private object GetWcfServiceObject(Type wcfServiceType)
        {
            if (wcfServiceType == null)
            {
                throw new ArgumentNullException("wcfServiceType");
            }

            var wcfService = iocContainer.GetInstance(wcfServiceType);
            if (wcfService == null)
            {
                throw new ApplicationException(string.Format("Unable to resolve WCF Service Type [{0}]", wcfServiceType));
            }

            return wcfService;
        }

        private void LogMessage(string message, Exception runtimeException = null, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine("{0}\n{1}", message, runtimeException);
#endif

            if (runtimeException == null)
            {
                rawLogger.LogInfo(loggerSource.LogLevel, "Domain Process Manager", loggerSource.Name, "System", methodName, message);
            }
            else
            {
                // TODO: Remove the string formatting
                rawLogger.LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                                                "Domain Process Manager", methodName,
                                                string.Format("{0}\n{1}", message, runtimeException), runtimeException);
            }
        }

    }
}
