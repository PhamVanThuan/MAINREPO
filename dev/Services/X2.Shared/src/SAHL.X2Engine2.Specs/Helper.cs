using System;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Specs
{
    public static class Helper
    {
        internal static InstanceDataModel GetInstanceDataModel(long instanceID)
        {
            return new InstanceDataModel(instanceID, 1, null, "InstanceName", "InstanceSubject", "WorkflowProvider", 1, @"SAHL\TestUser", DateTime.Now,
                null, null, null, @"SAHL\TestUser", null, null, null, null);
        }

        internal static WorkFlowDataModel GetWorkflowDataModel()
        {
            return new WorkFlowDataModel(1, 1, null, "WorkflowName", DateTime.Now, "X2DataTable", "ApplicationKey", 1, "Subject", 2);
        }

        internal static StateDataModel GetStateDataModel()
        {
            return new StateDataModel(1, 1, "StateName", 1, false, null, null, null, null);
        }

        internal static ActivityDataModel GetActivityDataModel()
        {
            return new ActivityDataModel(1, 1, "ActivityName", 1, 1, 2, false, 1, null, "ActivityMessage", null, null, null, string.Empty, null, null);
        }

        internal static Activity GetActivity()
        {
            return new Activity(0, "ActivityName", null, "FromState", 0, "ToState", 0, false);
        }
    }
}