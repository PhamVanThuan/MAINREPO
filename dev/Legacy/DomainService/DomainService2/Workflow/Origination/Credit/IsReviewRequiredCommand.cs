using System;

namespace DomainService2.Workflow.Origination.Credit
{
    public class IsReviewRequiredCommand : StandardDomainServiceCommand
    {
        public IsReviewRequiredCommand(Int64 instanceID, string activityName)
        {
            this.InstanceID = instanceID;
            this.ActivityName = activityName;
        }

        public Int64 InstanceID { get; protected set; }

        public string ActivityName { get; protected set; }

        public bool Result { get; set; }
    }
}