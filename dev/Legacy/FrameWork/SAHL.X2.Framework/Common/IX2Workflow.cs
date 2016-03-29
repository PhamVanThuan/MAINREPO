using System;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.X2.Framework.Common
{
    public interface IX2WorkFlow
    {
        bool OnEnterState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        bool OnExitState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        string GetForwardStateName(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        bool OnReturnState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        bool OnStartActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        bool OnCompleteActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, ref string AlertMessage);

        DateTime GetActivityTime(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        string GetStageTransition(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        string GetActivityMessage(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params);

        string GetDynamicRole(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, string RoleName, string WorkflowName);

        IX2WorkFlowDataProvider GetWorkFlowDataProvider();
    }
}