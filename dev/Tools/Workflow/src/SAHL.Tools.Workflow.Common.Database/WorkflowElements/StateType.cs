using System.Collections.Generic;
using Headspring;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public class StateType : Enumeration<StateType>
    {
        private StateType(int value, string displayName)
            : base(value, displayName)
        {
        }

        public static readonly StateType
            User = new StateType(1, "User");

        public static readonly StateType
            System = new StateType(2, "System");

        public static readonly StateType
            SystemDecision = new StateType(3, "SystemDecision");

        public static readonly StateType
            StartingPoint = new StateType(4, "StartingPoint");

        public static readonly StateType
            Archive = new StateType(5, "Archive");

        public static readonly StateType
            Hold = new StateType(6, "Hold");
    }
}