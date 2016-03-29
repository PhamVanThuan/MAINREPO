using SAHL.Core.Logging;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using System;
using System.Collections.Generic;

namespace SAHL.X2Engine2
{
    public abstract class X2TLSProcessBase : IX2TLSProcess
    {
        IX2Process process;
        IAppDomainProcessProxy proxy;
        ILogger metrics;
        public X2TLSProcessBase(IX2Process process, IAppDomainProcessProxy proxy, ILogger metrics)
        {
            this.process = process;
            this.proxy = proxy;
            this.metrics = metrics;
        }

        public Core.Data.IUIStatementsProvider GetUIStatementProvider()
        {
            return process.GetUIStatementProvider();
        }

        public Core.X2.IX2Map GetWorkflowMap(string workflowName)
        {
            var map = process.GetWorkflowMap(workflowName);
            return new X2TLSMap(map, proxy, metrics);
        }

        public IX2Process GetProcess
        {
            get { return process; }
        }

        public void Dispose()
        {
            this.process = null;
            this.proxy = null;
            this.metrics = null;
        }
    }

    public class X2TLSMap : IX2Map, IDisposable
    {
        IX2Map x2Map;
        private IAppDomainProcessProxy proxy;
        ILogger metrics;
        public X2TLSMap(IX2Map map, IAppDomainProcessProxy proxy, ILogger metrics)
        {
            this.x2Map = map;
            this.proxy = proxy;
            this.metrics = metrics;
        }

        private void UpdateTLS()
        {
            IDictionary<string, object> tls = proxy.GetTLSContents(metrics.GetType());
            foreach (var key in tls.Keys)
            {
                object value = tls[key];
                metrics.GetThreadLocalStore()[key] = value;
            }
        }

        public void Dispose()
        {
            this.x2Map = null;
            this.proxy = null;
            this.metrics = null;
        }

        public bool CompleteActivity(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages, ref string AlertMessage)
        {
            bool ret = x2Map.CompleteActivity(instance, contextualData, param, messages, ref AlertMessage);
            //UpdateTLS();
            return ret;
        }

        public bool EnterState(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.EnterState(instance, contextualData, param, messages);
        }

        public bool ExitState(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.ExitState(instance, contextualData, param, messages);
        }

        public string GetActivityMessage(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.GetActivityMessage(instance, contextualData, param, messages);
        }

        public DateTime GetActivityTime(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.GetActivityTime(instance, contextualData, param, messages);
        }

        public Core.X2.Providers.IX2ContextualDataProvider GetContextualData(long instanceID)
        {
            return x2Map.GetContextualData(instanceID);
        }

        public string GetDynamicRole(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, string roleName, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.GetDynamicRole(instance, contextualData, roleName, param, messages);
        }

        public string GetForwardStateName(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualDataProvider, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.GetForwardStateName(instance, contextualDataProvider, param, messages);
        }

        public string GetStageTransition(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.GetStageTransition(instance, contextualData, param, messages);
        }

        public bool OnReturnState(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.OnReturnState(instance, contextualData, param, messages);
        }

        public bool StartActivity(Core.Data.Models.X2.InstanceDataModel instance, Core.X2.Providers.IX2ContextualDataProvider contextualData, IX2Params param, Core.SystemMessages.ISystemMessageCollection messages)
        {
            return x2Map.StartActivity(instance, contextualData, param, messages);
        }
    }
}
