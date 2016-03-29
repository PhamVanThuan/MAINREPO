using System.Collections.Generic;
using Headspring;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public class ActivityType : Enumeration<ActivityType>
    {
        private ActivityType(int value, string displayName)
            : base(value, displayName)
        {
        }

        public static readonly ActivityType
            User = new ActivityType(1, "User");

        public static readonly ActivityType
            Decision = new ActivityType(2, "Decision");

        public static readonly ActivityType
            External = new ActivityType(3, "External");

        public static readonly ActivityType
            Timed = new ActivityType(4, "Timed");

        public static readonly ActivityType
            CallWorkFlow = new ActivityType(9, "CallWorkFlow");

        public static readonly ActivityType
            ReturnWorkflow = new ActivityType(10, "ReturnWorkflow");
    }
}