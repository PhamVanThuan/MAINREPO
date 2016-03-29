using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework
{
    public class V3ServiceCommon : IV3ServiceCommon
    {
        public V3ServiceCommon()
        {

        }

        public X2ServiceResponse CreateWorkflowCase(ISystemMessageCollection systemMessageCollection, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
            var currentPrincipal = SAHLPrincipal.GetCurrent();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(currentPrincipal);

            var currentX2ActivityInfo = x2Service.GetX2Info(currentPrincipal);
            if (currentX2ActivityInfo == null || String.IsNullOrEmpty(currentX2ActivityInfo.SessionID))
                x2Service.LogIn(currentPrincipal);

            X2ServiceResponse response = x2Service.CreateWorkFlowInstanceWithComplete(currentPrincipal, ProcessName, (-1).ToString(), WorkFlowName, ActivityName, InputFields, IgnoreWarnings);
            return response;
        }

        public void HandleSystemMessages(ISystemMessageCollection systemMessageCollection)
        {
            V3ServiceManager.Instance.HandleSystemMessages(systemMessageCollection);
        }
    }
}
