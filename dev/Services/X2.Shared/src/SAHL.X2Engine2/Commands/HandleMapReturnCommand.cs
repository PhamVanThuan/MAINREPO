using System;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Commands
{
    public class HandleMapReturnCommand : RuleCommand, IDontDecorateServiceCommand
    {
        public ISystemMessageCollection Messages { get; set; }

        public string ActivityName { get; set; }

        public bool IgnoreWarnings { get; set; }

        public Int64 InstanceId { get; set; }

        public WorkflowMapCodeSectionEnum WorkflowMapCodeSectionEnum { get; set; }

        /// <summary>
        /// Has a default param for ignore warnings as we havnt implemented this on any of the x2request messages
        /// </summary>
        /// <param name="result"></param>
        /// <param name="messages"></param>
        /// <param name="activityName"></param>
        /// <param name="ignoreWarnings"></param>
        public HandleMapReturnCommand(Int64 instanceId, bool result, ISystemMessageCollection messages, string activityName, WorkflowMapCodeSectionEnum workflowMapCodeSectionEnum, bool ignoreWarnings = true)
        {
            this.InstanceId = instanceId;
            this.Result = result;
            this.Messages = messages;
            this.ActivityName = activityName;
            this.IgnoreWarnings = ignoreWarnings;
            this.WorkflowMapCodeSectionEnum = workflowMapCodeSectionEnum;
        }
    }
}