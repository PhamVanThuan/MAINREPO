using SAHL.Core.Data.Models.X2;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Providers;
using System;

namespace SAHL.Core.X2
{
    public interface IX2Map
    {
        bool StartActivity(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages);

        bool CompleteActivity(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages, ref string AlertMessage);

        bool ExitState(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages);

        bool EnterState(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages);

        string GetStageTransition(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages);

        string GetActivityMessage(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages);

        IX2ContextualDataProvider GetContextualData(long instanceID);

        string GetForwardStateName(InstanceDataModel instance, IX2ContextualDataProvider contextualDataProvider, IX2Params param, ISystemMessageCollection messages);

        string GetDynamicRole(InstanceDataModel instance, IX2ContextualDataProvider contextualData, string roleName, IX2Params param, ISystemMessageCollection messages);

        DateTime GetActivityTime(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages);

        bool OnReturnState(InstanceDataModel instance, IX2ContextualDataProvider contextualData, IX2Params param, ISystemMessageCollection messages);
    }
}